using ECommerce.Business.Models.Dtos.PaymentCards;
using FluentValidation;

namespace ECommerce.Business.Validations.FluentValidations.PaymentCards
{
    public class PaymentCardUpdateDtoValidator : AbstractValidator<PaymentCardUpdateDto>
    {
        public PaymentCardUpdateDtoValidator()
        {
            RuleFor(_ => _.Id)
                .NotNull()
                .WithMessage("Id boş olamaz.")
                .NotEmpty()
                .WithMessage("Id boş olmaz.");

            RuleFor(_ => _.NameSurname)
                .NotNull()
                .WithMessage("Ad soyad boş olamaz.")
                .NotEmpty()
                .WithMessage("Ad soyad boş olmaz.")
                .MinimumLength(5)
                .WithMessage("Ad Soyad en az 5 karakter içermelidir.")
                .MaximumLength(70)
                .WithMessage("Ad Soyad en fazle 70 karakter içermelidir.");

            RuleFor(_ => _.CardNumber)
                .NotNull()
                .WithMessage("Kart numarası boş olamaz.")
                .NotEmpty()
                .WithMessage("Kart numarası boş olmaz.")
                .Length(16)
                .WithMessage("Kart numarası 16 karakter içermelidir.");

            RuleFor(_ => _.CVV)
               .NotNull()
               .WithMessage("CVV boş olamaz.")
               .NotEmpty()
               .WithMessage("CVV boş olmaz.")
               .Length(3)
               .WithMessage("CVV 3 karakter içermelidir.");

            RuleFor(_ => _.ExpireDate)
               .NotNull()
               .WithMessage("CVV boş olamaz.")
               .NotEmpty()
               .WithMessage("CVV boş olmaz.");
        }
    }
}
