using FluentValidation;
using IKitaplık.Entities.Concrete;

public class StudentValidator : AbstractValidator<Student>
{
    public StudentValidator()
    {
        RuleFor(student => student.Name)
            .NotEmpty().WithMessage("Öğrenci adı boş olamaz.")
            .Length(2, 100).WithMessage("Öğrenci adı 2 ile 100 karakter arasında olmalıdır.");

        RuleFor(student => student.StudentNumber)
            .GreaterThan(0).WithMessage("Öğrenci numarası geçerli bir değer olmalıdır.");

        RuleFor(student => student.Class)
            .NotEmpty().WithMessage("Sınıf boş olamaz.");

        RuleFor(student => student.TelephoneNumber)
            .NotEmpty().WithMessage("Telefon numarası boş olamaz.");

        RuleFor(student => student.EMail)
            .NotEmpty().WithMessage("E-posta adresi boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

        RuleFor(student => student.NumberofBooksRead)
            .GreaterThanOrEqualTo(0).WithMessage("Okunan kitap sayısı sıfır veya daha büyük olmalıdır.");
    }
}
