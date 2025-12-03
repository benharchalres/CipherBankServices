using CipherBank.AuthService.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Application.Validators
{
    public class RegisterRequestDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterRequestDtoValidator() 
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required.");

            RuleFor(x => x.Email)
                            .NotEmpty().WithMessage("Email is required.")
                            .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).MaximumLength(15).WithMessage("Password must be range of 8 t0 15 characters.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).+$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one special character.");

            RuleFor(x => x.ComfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Equal(x => x.Password).WithMessage("Passwords do not match.");

        }
    }
}
