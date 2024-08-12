using FluentValidation;
using IKitaplık.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Validations.FluentValidations
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(b => b.Name)
                .NotEmpty().WithMessage("İsim alanı boş olamaz")
                .Length(5,100).WithMessage("İsim alanı 5 ile 100 karakter arası olmalıdır");

            RuleFor(b => b.Piece)
                .GreaterThan(0).WithMessage("Kitap adedi 0'dan büyük olmalıdır");

            RuleFor(b => b.CategoryId)
                .GreaterThan(0).WithMessage("Bir kategori seçiniz");

            RuleFor(b => b.Barcode)
                .NotEmpty().WithMessage("Barkod giriniz");

            RuleFor(b => b.Writer)
                .NotEmpty().WithMessage("Yazar alanı boş olamaz")
                .Length(5, 100).WithMessage("Yazar alanı 5 ile 100 karakter arası olmalıdır");
        }
    }
}
