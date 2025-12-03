using CipherBank.AuthService.Application.Service.Concrete;
using CipherBank.AuthService.Application.Service.Contract;
using CipherBank.AuthService.Identity.Password;
using CipherBank.AuthService.Identity.TokenService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Identity
{
    public static class InfrastructureIdentityDependencyInjection
    {
        public static IServiceCollection AddInfrastructureIdentity(this IServiceCollection services,IConfiguration configuration)
        {
            // Register Infrastructure-level services (interfaces only)

            services.AddScoped<IPasswordHasherService, BcryptPasswordHasher>();
            services.AddScoped<ITokenService,JwtTokenService>();
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

            return services;
        }
    }
}
