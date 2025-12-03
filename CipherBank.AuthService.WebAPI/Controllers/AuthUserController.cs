using CipherBank.AuthService.Application.Repository.Contract;
using CipherBank.AuthService.Application.Service.Contract;
using CipherBank.AuthService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace CipherBank.AuthService.WebAPI.Controllers
{

    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public sealed class AdminUsersController : ControllerBase
    {
        private readonly IAuthUserService _authUserService;
        private readonly IAuthUserRepository _authUserRepository;
        public AdminUsersController(IAuthUserService authUserService, IAuthUserRepository authUserRepository)
        {
            _authUserService = authUserService;
            _authUserRepository = authUserRepository;
        }

        [HttpPost("{id:guid}/roles")]
        public async Task<IActionResult> AssignRole(Guid id, [FromBody] string role, CancellationToken ct)
        {
            await _authUserService.AssignRoleAsync(id, role);
            return Ok(new { message = "Successfully role has been changed",Userid = id });
        }

        [HttpPost("{id:guid}/lock")]
        public async Task<IActionResult> Lock(Guid id, CancellationToken ct)
        {
            await _authUserService.LockUserAsync(id);
            return Ok(new { message = "Successfully UserAccount Locked", Userid = id });
        }

        [HttpPost("{id:guid}/unlock")]
        public async Task<IActionResult> Unlock(Guid id, CancellationToken ct)
        {
            await _authUserService.UnLockUserAsync(id);
            return Ok(new { message = "Successfully UserAccount Unlocked", Userid = id });
        }
        [HttpGet("AllUsers")]
        public async Task<List<User>> GetAllUsers()
        {
            return await _authUserRepository.GetAllAsync();
        }

    }

}
