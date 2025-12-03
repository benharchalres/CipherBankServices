using CipherBank.AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Application.Service.Contract;

public interface ITokenService
{
    Task<TokenPair> GenerateAsync(User user, CancellationToken ct);
    Task<TokenPair> RefreshAsync(string refreshToken, CancellationToken ct);
    Task RevokeAsync(string refreshToken, CancellationToken ct);
    bool TryValidateAccessToken(string accessToken, out IEnumerable<Claim> claims);
    
}
public sealed record TokenPair(string AccessToken, string RefreshToken, int ExpiresIn);
