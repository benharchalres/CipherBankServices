using CipherBank.AuthService.Application.Repository.Contract;
using CipherBank.AuthService.Application.Service.Contract;
using CipherBank.AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CipherBank.AuthService.Infrastructure.Persistence.Repository.Concrete
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _authDbContext;
       // private readonly IPasswordHasherService _passwordHasherService ;
        public UserRepository(AuthDbContext authDbContext) 
        {
            _authDbContext = authDbContext;
        }
        public async Task<User> GetByUserNameAsync(string Username,CancellationToken ct)
        {
            var user = await _authDbContext.Users
           .Include(u => u.Roles)
           .FirstOrDefaultAsync(u => u.Email == Username, ct);

            return user;
        }
        public async Task AddAsync(User entity)
        { 
            await _authDbContext.Users.AddAsync(entity);
            await _authDbContext.SaveChangesAsync();
        }
        public async Task<Role> GetDefaultRoleAsync(string roleName)
        {
            var role = await _authDbContext.Roles.FirstOrDefaultAsync(role => role.Name == roleName);
            return role;
        }
        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await _authDbContext.Users.Include(u=>u.Roles).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
    }
}
