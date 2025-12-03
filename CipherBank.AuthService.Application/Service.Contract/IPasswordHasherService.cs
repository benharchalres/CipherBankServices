using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Application.Service.Contract
{
    public interface IPasswordHasherService
    {
        string Hash(string password);
        bool VerifyHash(string hashPassword, string Password);
    }
}
