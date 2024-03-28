using ECommerce.Business.Models.Dtos.PaymentCards;
using FluentValidation;

namespace ECommerce.Business.Validations.FluentValidations.PaymentCards
{
    public class PaymentCardUpdateDtoValidator : AbstractValidator<PaymentCardUpdateDto>
    {
        public PaymentCardUpdateDtoValidator()
        {
            RuleFor(_ => _.Id)
                .NotEmpty()
                .WithMessage("Id boş olmaz.");

            RuleFor(_ => _.NameSurname)
                .NotEmpty()
                .WithMessage("Ad soyad boş olmaz.")
                .MinimumLength(5)
                .WithMessage("Ad Soyad en az 5 karakter içermelidir.")
                .MaximumLength(70)
                .WithMessage("Ad Soyad en fazle 70 karakter içermelidir.");

            RuleFor(_ => _.CardNumber)
                .NotEmpty()
                .WithMessage("Kart numarası boş olmaz.")
                .Length(16)
                .WithMessage("Kart numarası 16 karakter içermelidir.");

            RuleFor(_ => _.CVV)
               .NotEmpty()
               .WithMessage("CVV boş olmaz.")
               .Length(3)
               .WithMessage("CVV 3 karakter içermelidir.");

            RuleFor(_ => _.ExpireDate)
               .NotEmpty()
               .WithMessage("CVV boş olmaz.");
        }
    }
}
