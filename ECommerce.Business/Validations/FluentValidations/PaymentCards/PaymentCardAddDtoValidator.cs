using ECommerce.Business.Models.Dtos.PaymentCards;
using FluentValidation;

namespace ECommerce.Business.Validations.FluentValidations.PaymentCards
{
    public class PaymentCardAddDtoValidator : AbstractValidator<PaymentCardAddDto>
    {
        public PaymentCardAddDtoValidator()
        {
            RuleFor(_ => _.NameSurname)
                .NotNull()
                .WithMessage("Ad soyad boş olamaz.")
                .NotEmpty()
                .WithMessage("Ad soyad boş olamaz.")
                .MinimumLength(5)
                .WithMessage("Ad Soyad en az 5 karakter içermelidir.")
                .MaximumLength(70)
                .WithMessage("Ad Soyad en fazle 70 karakter içermelidir.");

            RuleFor(_ => _.CardNumber)
                .NotNull()
                .WithMessage("Kart numarası boş olamaz.")
                .NotEmpty()
                .WithMessage("Kart numarası boş olamaz.")
                .Length(16)
                .WithMessage("Kart numarası 16 karakter içermelidir.");

            RuleFor(_ => _.CVV)
               .NotNull()
               .WithMessage("CVV boş olamaz.")
               .NotEmpty()
               .WithMessage("CVV boş olamaz.")
               .Length(3)
               .WithMessage("CVV 3 karakter içermelidir.");

            RuleFor(_ => _.ExpireDate)
               .NotNull()
               .WithMessage("Geçerlilik tarihi boş olamaz.")
               .NotEmpty()
               .WithMessage("Geçerlilik tarihi boş olmaz.");
        }
    }
}
