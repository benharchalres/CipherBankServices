using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Application.Service.Contract
{
     public interface IAuthUserService
    {
        Task AssignRoleAsync(Guid id, string role);
        Task LockUserAsync(Guid id);
        Task UnLockUserAsync(Guid id);

    }
}
