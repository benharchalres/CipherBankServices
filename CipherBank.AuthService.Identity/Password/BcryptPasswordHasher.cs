using CipherBank.AuthService.Application.Service.Contract;

namespace CipherBank.AuthService.Identity.Password
{
    public sealed  class BcryptPasswordHasher : IPasswordHasherService
    {
        public  string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password,workFactor:10);
        }

        public  bool VerifyHash(string password, string hashPassword)
        {
            bool isPasswordValid =  BCrypt.Net.BCrypt.Verify(password, hashPassword);

            return isPasswordValid ? true : false;
        }
    }
}
