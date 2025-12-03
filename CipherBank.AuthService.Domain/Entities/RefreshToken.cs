using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; } 
        public string Token { get; set; } = default!;
        public DateTimeOffset Expiry { get; set; }
        public Guid UserId { get; set; }
        public bool IsRevoked { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? RevokedAt { get;  set; }
        public string? ReplacedByToken { get;  set; }

        public RefreshToken()
        {
            
        }
        public RefreshToken(string token, DateTimeOffset expiry, Guid userId)
        {
            Id = Guid.NewGuid();
            Token = token;
            Expiry = expiry;
            UserId = userId;
            CreatedAt = DateTimeOffset.UtcNow;
            IsRevoked = false;
        }

        public void Revoke() => IsRevoked = true;

    }

}
