using CipherBank.AuthService.Application.Repository.Contract;
using CipherBank.AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Infrastructure.Persistence.Repository.Concrete
{
    public class AuthUserRepository : IAuthUserRepository
    {
        private readonly AuthDbContext _authDbContext;
        private readonly IUserRepository _userRepository;
        public AuthUserRepository(AuthDbContext authDbContext,IUserRepository userRepository) 
        { 
            _authDbContext = authDbContext;
            _userRepository = userRepository;
        }
        public async Task AssignRoleAsync(Guid id, string roleName)
        {
            var user = await _authDbContext.Users.FindAsync(id);
            var role = await _userRepository.GetDefaultRoleAsync(roleName);
            user.AddRole(role);
            await _authDbContext.SaveChangesAsync();
        }
        public async Task LockUserAsync(Guid id)
        {
            var user = await _authDbContext.Users.FindAsync(id);
            user.IsLocked = true;
            await _authDbContext.SaveChangesAsync();

        }
        public async Task UnLockUserAsync(Guid id)
        {
            var user = await _authDbContext.Users.FindAsync(id);
            user.IsLocked = true;
            await _authDbContext.SaveChangesAsync();
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _authDbContext.Users.Include(user => user.Roles).ToListAsync();
        }
    }
}
