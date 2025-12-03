using CipherBank.AuthService.Application.DTOs;
using CipherBank.AuthService.Application.Repository.Contract;
using CipherBank.AuthService.Application.Service.Contract;
using CipherBank.AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Application.Service.Concrete
{
   
    public class UserService : IUserService
    {
        private readonly IGenericService _genericService;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly ITokenService _tokenService;
       
        public UserService(IUserRepository userRepository, IGenericService genericService,IPasswordHasherService passwordHasherService,ITokenService tokenService)
        {
            _userRepository = userRepository;
            _genericService = genericService;
            _passwordHasherService = passwordHasherService;
            _tokenService = tokenService;
        }
        public async Task<ResponseDto<bool>> RegisterAsync(RegisterUserDto registerUserDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = registerUserDto.UserName,
                Email = registerUserDto.Email,
                PasswordHash = _passwordHasherService.Hash(registerUserDto.Password)
            };
            var role = await _userRepository.GetDefaultRoleAsync("Customer");
            user.AddRole(role);
            await _userRepository.AddAsync(user);
            return _genericService.BuildResponseModel<bool>("User successfully register.Please login to Continue Next", true);
        }
        
        public async Task< ResponseDto<TokenPair>> LoginAsync(LoginRequestDto loginRequestDto,CancellationToken ct)
        {
            var user = await _userRepository.GetByUserNameAsync(loginRequestDto.Email,ct);

            if(user==null)
            {
                return _genericService.BuildResponseModel <TokenPair>("User Account not exist. Please Register.",null,false,"404");
            }

            if(user.IsLocked)
            {
                return _genericService.BuildResponseModel<TokenPair>("Current User Account is Locked. Please Contact Bank.", null, false, "403");
            }

            if (_passwordHasherService.VerifyHash(loginRequestDto.Password, user.PasswordHash))
            {
                var token = await _tokenService.GenerateAsync(user, ct);
                return _genericService.BuildResponseModel<TokenPair>("User successfully login", token);
            }
            else
            {
                return _genericService.BuildResponseModel<TokenPair>("InValid Email and Password.", null, false,"401");
            }
                
        }

        public async Task<TokenPair> RefreshTokenAsync(string refresh,CancellationToken ct)
        {
            return await _tokenService.RefreshAsync(refresh, ct);
        }
        public async Task LogoutAsync(string refreshToken,CancellationToken ct)
        {
            await _tokenService.RevokeAsync(refreshToken, ct);
        }
    }
}
