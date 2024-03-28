using ECommerce.Business.Models.Dtos.Brands;
using FluentValidation;

namespace ECommerce.Business.Validations.FluentValidations.Brands
{
    public class BrandUpdateDtoValidator : AbstractValidator<BrandUpdateDto>
    {
        public BrandUpdateDtoValidator()
        {
            RuleFor(_ => _.Id)
               .NotEmpty()
               .WithMessage("Marka ismi boş olamaz.");

            RuleFor(_ => _.Name)
               .NotEmpty()
               .WithMessage("Marka ismi boş olamaz.")
               .MinimumLength(2)
               .WithMessage("Marka ismi en az 2 karakter içermelidir.")
               .MaximumLength(50)
               .WithMessage("Marka ismi en fazle 50 karakter içermelidir.");
        }
    }
}
