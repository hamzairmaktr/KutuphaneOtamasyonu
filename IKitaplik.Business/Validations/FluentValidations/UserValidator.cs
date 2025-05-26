using FluentValidation;
using IKitaplik.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Validations.FluentValidations
{
    public class UserValidator:AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Username).NotEmpty().WithMessage("User name cannot be empty.")
                                      .Length(2, 50).WithMessage("Full name must be between 2 and 50 characters.");
            RuleFor(u => u.FullName).NotEmpty().WithMessage("Last name cannot be empty.")
                                     .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");
            RuleFor(u => u.Email).NotEmpty().WithMessage("Email cannot be empty.")
                                  .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(u => u.PasswordHash).NotEmpty().WithMessage("Password cannot be empty.")
                                     .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
