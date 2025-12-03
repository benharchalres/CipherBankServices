using CipherBank.AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Application.Repository.Contract
{
    public interface IAuthUserRepository
    {
        Task AssignRoleAsync(Guid id, string roleName);
        Task LockUserAsync(Guid id);
        Task UnLockUserAsync(Guid id);

        Task<List<User>> GetAllAsync();
    }
}
