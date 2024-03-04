using ECommerce.Business.Models.Dtos.Auth;
using FluentValidation;

namespace ECommerce.Business.Validations.FluentValidations.Auth
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(_ => _.Mail)
                .NotNull()
                .WithMessage("Mail adresi boş olamaz.")
                .NotEmpty()
                .WithMessage("Mail adresi boş olamaz")
                .MinimumLength(12)
                .WithMessage("Mail adresi en az 12 karakter içermelidir.")
                .MaximumLength(50)
                .WithMessage("Mail adresi en fazla 50 karakter içermelidir.");

            RuleFor(_ => _.Password)
                .NotNull()
                .WithMessage("Şifre boş olamaz.")
                .NotEmpty()
                .WithMessage("Şifre boş olamaz")
                .MinimumLength(7)
                .WithMessage("Şifre en az 7 karakter içermelidir.")
                .MaximumLength(25)
                .WithMessage("Şifre en fazla 25 karakter içermelidir.");
        }
    }
}
