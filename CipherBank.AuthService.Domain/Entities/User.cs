using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Domain.Entities
{
    public class User
    {
        public Guid Id { get;  set; }
        public string UserName { get;  set; } = default!;
        public long PhoneNumber { get; set; }  
        public string? Email { get;  set; } = default!;
        public string? PasswordHash { get;  set; } = default!;
        public bool IsLocked { get;  set; }
        public bool MFAEnabled { get;  set; }
        public ICollection<Role> Roles { get; private set; } = new List<Role>();

        public User() { }
        public User(Guid id, string username, string email, string passwordHash)
                => (Id, UserName, Email, PasswordHash) = (id, username, email, passwordHash);
        public void EnableMfa() => MFAEnabled = true;
        public void LockAccount() => IsLocked = true;
        public void AddRole(Role role) => Roles.Add(role);
    }
}
