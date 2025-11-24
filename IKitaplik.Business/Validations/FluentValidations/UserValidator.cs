using FluentValidation;
using IKitaplik.Entities.DTOs.UserDTOs;

namespace IKitaplik.Business.Validations.FluentValidations
{
    public class UserValidator : AbstractValidator<UserRegisterDto>
    {
        public UserValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz.")
                .Length(2, 50).WithMessage("Kullanıcı adı 2 ile 50 karakter arasında olmalıdır.");

            RuleFor(u => u.FullName)
                .NotEmpty().WithMessage("Ad soyad alanı boş olamaz.")
                .Length(2, 50).WithMessage("Ad soyad 2 ile 50 karakter arasında olmalıdır.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("E-posta boş olamaz.")
                .EmailAddress().WithMessage("Geçersiz e-posta formatı.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
                .Must(NotContainInvalidChars).WithMessage("Şifre geçersiz karakter içeriyor. (' \" # ; < > karakterleri kullanılamaz.)");
        }

        private bool NotContainInvalidChars(string password)
        {
            var invalidChars = new[] { '\'', '"', '#', ';', '<', '>' };
            return !password.Any(c => invalidChars.Contains(c));
        }
    }
}
