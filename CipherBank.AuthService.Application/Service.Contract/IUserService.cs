using CipherBank.AuthService.Application.DTOs;
using CipherBank.AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Application.Service.Contract
{
    public interface IUserService
    {
        Task<ResponseDto<bool>> RegisterAsync(RegisterUserDto entity);
        Task<ResponseDto<TokenPair>> LoginAsync(LoginRequestDto entity,CancellationToken ct);
        Task<TokenPair> RefreshTokenAsync(string refreshToken,CancellationToken ct);
        Task LogoutAsync(string refreshToken,CancellationToken ct);
        
    }
}
