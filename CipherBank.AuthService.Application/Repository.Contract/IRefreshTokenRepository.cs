using CipherBank.AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Application.Repository.Contract
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken, CancellationToken ct);
        Task<RefreshToken?> GetByTokenAsync(string refreshTokenValue, CancellationToken ct);
        Task RevokeAsync(Guid refreshTokenId, DateTimeOffset? revokedAt, CancellationToken ct);
        Task<RefreshToken> RotateAsync(RefreshToken existing, RefreshToken newRefreshToken, CancellationToken ct);
        Task<int> PurgeAsync(DateTimeOffset olderThan, CancellationToken ct);
    }
}
