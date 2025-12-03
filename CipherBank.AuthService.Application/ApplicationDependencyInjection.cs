using CipherBank.AuthService.Application.Service.Concrete;
using CipherBank.AuthService.Application.Service.Contract;
using CipherBank.AuthService.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // service for Application
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGenericService, GenericService>();
            services.AddScoped<IAuthUserService, AuthUserService>();
            services.AddValidatorsFromAssemblyContaining<LoginRequestDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<RegisterRequestDtoValidator>();
            return services;
        }
    }
}
