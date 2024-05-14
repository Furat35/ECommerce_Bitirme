using ECommerce.Business.Models.Dtos.Users;
using FluentValidation;

namespace ECommerce.Business.Validations.FluentValidations.Users
{
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(_ => _.Name)
               .NotEmpty()
               .WithMessage("İsim boş olamaz!");

            RuleFor(_ => _.Surname)
                .NotEmpty()
                .WithMessage("Soyad boş olamaz!");

            RuleFor(_ => _.Mail)
                .NotEmpty()
                .WithMessage("Mail adresi boş olamaz!");

            RuleFor(_ => _.Phone)
                .NotEmpty()
                .WithMessage("Telefon boş olamaz!");
        }
    }
}
