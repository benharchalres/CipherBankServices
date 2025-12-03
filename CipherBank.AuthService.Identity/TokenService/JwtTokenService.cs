using CipherBank.AuthService.Application.Repository.Contract;
using CipherBank.AuthService.Application.Service.Contract;
using CipherBank.AuthService.Domain.Entities;
using CipherBank.AuthService.Identity.Password;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CipherBank.AuthService.Identity.TokenService
{
    public class JwtTokenService : ITokenService
    {
        private readonly JwtOptions _options;
        private readonly IRefreshTokenRepository _refreshTokens;
        private readonly IUserRepository _userRepository;

        public JwtTokenService(IOptions<JwtOptions> options, IRefreshTokenRepository refreshTokens,IUserRepository userRepository)
        {
            _options = options.Value;
            _refreshTokens = refreshTokens;
            _userRepository = userRepository;
        }

        public async Task<TokenPair> GenerateAsync(User user, CancellationToken ct)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User parameter not be null",nameof(user));
            }
            // 1. Create JWT access token
            var accessToken = CreateJwtToken(user);

            // 2. Create refresh token
            var refreshTokenValue = GenerateSecureToken();
            var refreshToken = new RefreshToken(refreshTokenValue, DateTimeOffset.UtcNow.AddDays(_options.RefreshTokenDays), user.Id);

            await _refreshTokens.AddAsync(refreshToken, ct);

            return new TokenPair(accessToken, refreshTokenValue, _options.AccessTokenMinutes * 60);
        }

        public async Task<TokenPair> RefreshAsync(string refreshTokenValue, CancellationToken ct)
        {
            var existing = await _refreshTokens.GetByTokenAsync(refreshTokenValue, ct);
            if (existing is null || existing.IsRevoked || existing.Expiry <= DateTimeOffset.UtcNow)
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            // Rotate token
            var newRefreshTokenValue = GenerateSecureToken();
            var newRefreshToken = new RefreshToken(newRefreshTokenValue, DateTimeOffset.UtcNow.AddDays(_options.RefreshTokenDays), existing.UserId);

            await _refreshTokens.RotateAsync(existing, newRefreshToken, ct);

            // Create new access token
            var user = await _userRepository.GetByIdAsync(existing.UserId); // Ideally fetch user from DB
            var accessToken = CreateJwtToken(user);

            return new TokenPair(accessToken, newRefreshTokenValue, _options.AccessTokenMinutes * 60);
        }

        public async Task RevokeAsync(string refreshTokenValue, CancellationToken ct)
        {
            var existing = await _refreshTokens.GetByTokenAsync(refreshTokenValue, ct);
            if (existing is null) return;

            await _refreshTokens.RevokeAsync(existing.Id, DateTimeOffset.UtcNow, ct);
        }

        public bool TryValidateAccessToken(string accessToken, out IEnumerable<Claim> claims)
        {
            claims = Array.Empty<Claim>();
            var handler = new JwtSecurityTokenHandler();
            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _options.Issuer,
                ValidAudience = _options.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey))
            };

            try
            {
                var principal = handler.ValidateToken(accessToken, validationParams, out _);
                claims = principal.Claims;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string CreateJwtToken(User user)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_options.SecretKey);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName),
                new Claim("roles", string.Join(",", user.Roles.Select(r => r.Name)))
            };

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes),
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        private static string GenerateSecureToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }

    public sealed class JwtOptions
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
        public int AccessTokenMinutes { get; set; } = 15;
        public int RefreshTokenDays { get; set; } = 7;
    }
}
 
