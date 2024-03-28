using ECommerce.Business.Models.Dtos.PaymentCards;
using FluentValidation;

namespace ECommerce.Business.Validations.FluentValidations.PaymentCards
{
    public class PaymentCardAddDtoValidator : AbstractValidator<PaymentCardAddDto>
    {
        public PaymentCardAddDtoValidator()
        {
            RuleFor(_ => _.NameSurname)
                .NotEmpty()
                .WithMessage("Ad soyad boş olamaz.")
                .MinimumLength(5)
                .WithMessage("Ad Soyad en az 5 karakter içermelidir.")
                .MaximumLength(70)
                .WithMessage("Ad Soyad en fazle 70 karakter içermelidir.");

            RuleFor(_ => _.CardNumber)
                .NotEmpty()
                .WithMessage("Kart numarası boş olamaz.")
                .Length(16)
                .WithMessage("Kart numarası 16 karakter içermelidir.");

            RuleFor(_ => _.CVV)
               .NotEmpty()
               .WithMessage("CVV boş olamaz.")
               .Length(3)
               .WithMessage("CVV 3 karakter içermelidir.");

            RuleFor(_ => _.ExpireDate)
               .NotEmpty()
               .WithMessage("Geçerlilik tarihi boş olmaz.");
        }
    }
}
