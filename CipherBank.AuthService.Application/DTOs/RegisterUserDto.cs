using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Application.DTOs
{
    public class RegisterUserDto
    {
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ComfirmPassword { get; set; } = default!;
    }
}
