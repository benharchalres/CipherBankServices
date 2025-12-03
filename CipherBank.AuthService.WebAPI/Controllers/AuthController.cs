using CipherBank.AuthService.Application.DTOs;
using CipherBank.AuthService.Application.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CipherBank.AuthService.WebAPI.Controllers
{

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
       
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto, CancellationToken ct)
        {
            var result = await _userService.LoginAsync(dto, ct);
            return Ok(result); 
        }

        [HttpPost("register")]
        public async Task<ResponseDto<bool>> Register(RegisterUserDto registerUserDto)
        {
            return await _userService.RegisterAsync(registerUserDto);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken, CancellationToken ct)
        {
           var pair = await _userService.RefreshTokenAsync(refreshToken,ct) ;
            return Ok(pair);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] string refreshToken, CancellationToken ct)
        {
            await _userService.LogoutAsync(refreshToken, ct);
            return Ok(new { message = "Logged out" });
        }

    }

}
