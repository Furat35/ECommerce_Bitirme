using ECommerce.Business.Models.Dtos.Auth;
using FluentValidation;

namespace ECommerce.Business.Validations.FluentValidations.Auth
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(_ => _.Name)
                .NotNull()
                .WithMessage("İsim boş olamaz.")
                .NotEmpty()
                .WithMessage("İsim boş olamaz.")
                .MinimumLength(2)
                .WithMessage("İsim en az 2 karakter içermelidir.")
                .MaximumLength(50)
                .WithMessage("İsim en fazla 50 karakter içermelidir.");

            RuleFor(_ => _.Surname)
               .NotNull()
               .WithMessage("Soyad boş olamaz.")
               .NotEmpty()
               .WithMessage("Soyad boş olamaz.")
               .MinimumLength(2)
               .WithMessage("Soyad en az 2 karakter içermelidir.")
               .MaximumLength(50)
               .WithMessage("Soyad en fazla 50 karakter içermelidir.");

            RuleFor(_ => _.Mail)
               .NotNull()
               .WithMessage("Mail boş olamaz.")
               .MinimumLength(12)
               .WithMessage("Mail en az 12 karakter içermelidir.")
               .MaximumLength(50)
               .WithMessage("Mail en fazla 50 karakter içermelidir.");

            RuleFor(_ => _.Password)
                .NotNull()
                .WithMessage("Şifre boş olamaz.")
                .MinimumLength(7)
                .WithMessage("Şifre en az 7 karakter içermelidir.")
                .MaximumLength(25)
                .WithMessage("Şifre en fazla 25 karakter içermelidir.");
        }
    }
}
