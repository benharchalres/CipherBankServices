using CipherBank.AuthService.Application.Repository.Contract;
using CipherBank.AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Infrastructure.Persistence.Repository.Concrete
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AuthDbContext _db;
        public RefreshTokenRepository(AuthDbContext db) => _db = db;


        public async Task AddAsync(RefreshToken token, CancellationToken ct)
        {
            await _db.RefreshTokens.AddAsync(token, ct);
            await _db.SaveChangesAsync(ct);
        }

        public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct)
        {
            return _db.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(rt => rt.Token == token, ct);
        }

        public async Task RevokeAsync(Guid refreshTokenId, DateTimeOffset? revokedAt, CancellationToken ct)
        {
            var entity = await _db.RefreshTokens.FirstOrDefaultAsync(rt => rt.Id == refreshTokenId, ct);
            if (entity is null) return;

            entity.IsRevoked = true;

            await _db.SaveChangesAsync(ct);
        }

        public async Task<RefreshToken> RotateAsync(RefreshToken oldToken, RefreshToken newToken, CancellationToken ct)
        {
          
            if (oldToken.IsRevoked || oldToken.Expiry <= DateTimeOffset.UtcNow)
                throw new InvalidOperationException("Old refresh token is invalid or expired.");

    
            await using IDbContextTransaction tx = await _db.Database.BeginTransactionAsync(ct);

            var trackedOld = await _db.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Id == oldToken.Id, ct);

            if (trackedOld is null)
                throw new InvalidOperationException("Old refresh token not found.");

            trackedOld.IsRevoked = true;
            trackedOld.RevokedAt = DateTimeOffset.UtcNow;
            trackedOld.ReplacedByToken = newToken.Token; 

            await _db.RefreshTokens.AddAsync(newToken, ct);
            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            return newToken;
        }

 
        public async Task<int> PurgeAsync(DateTimeOffset olderThan, CancellationToken ct)
        {
            
            return await _db.RefreshTokens
                .Where(rt => rt.Expiry < olderThan || (rt.IsRevoked && (rt.RevokedAt ?? DateTimeOffset.MinValue) < olderThan))
                .ExecuteDeleteAsync(ct);
        }
    }
}
