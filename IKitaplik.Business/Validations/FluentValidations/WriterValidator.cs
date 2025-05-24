using FluentValidation;
using IKitaplik.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Validations.FluentValidations
{
    public class WriterValidator : AbstractValidator<Writer>
    {
        public WriterValidator()
        {
            RuleFor(p => p.WriterName).MinimumLength(5).WithMessage("Yazar adı en az 5 karakter olmalıdır");
        }
    }
}
