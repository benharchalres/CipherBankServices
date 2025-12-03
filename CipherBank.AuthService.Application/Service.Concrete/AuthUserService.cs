using CipherBank.AuthService.Application.Repository.Contract;
using CipherBank.AuthService.Application.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Application.Service.Concrete
{
    public class AuthUserService : IAuthUserService
    {
        private readonly IAuthUserRepository _authUserRepository;
        public AuthUserService(IAuthUserRepository authUserRepository) 
        {
            _authUserRepository = authUserRepository;
        }
        public async Task AssignRoleAsync(Guid id, string roleName)
        {
            
            await _authUserRepository.AssignRoleAsync(id, roleName);
        }
        public async Task LockUserAsync(Guid id)
        {
            await _authUserRepository.LockUserAsync(id);
        }
        public async Task UnLockUserAsync(Guid id)
        {
           await _authUserRepository.UnLockUserAsync(id);
        }
    }
}
