using CipherBank.AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Application.Repository.Contract
{
    public interface IUserRepository
    {
        Task AddAsync(User entity);
        Task<User> GetByUserNameAsync(string Username,CancellationToken ct);

        Task<Role> GetDefaultRoleAsync(string name);

        Task<User> GetByIdAsync(Guid id);

    }
}
