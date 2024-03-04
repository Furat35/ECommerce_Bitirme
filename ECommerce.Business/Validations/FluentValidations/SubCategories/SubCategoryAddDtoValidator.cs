using ECommerce.Business.Models.Dtos.SubCategories;
using FluentValidation;

namespace ECommerce.Business.Validations.FluentValidations.SubCategories
{
    public class SubCategoryAddDtoValidator : AbstractValidator<SubCategoryAddDto>
    {
        public SubCategoryAddDtoValidator()
        {
            RuleFor(_ => _.CategoryId)
                .NotEmpty()
                .WithMessage("Alt Kategori id'si boş olamaz.")
                .NotNull()
                .WithMessage("Alt Kategori id'si boş olamaz.");

            RuleFor(_ => _.Name)
               .NotEmpty()
               .WithMessage("Alt Kategori ismi boş olamaz.")
               .NotNull()
               .WithMessage("Alt Kategori ismi boş olamaz.")
               .MinimumLength(2)
               .WithMessage("Alt Kategori ismi en az 2 karakter içermelidir.")
               .MaximumLength(50)
               .WithMessage("Alt Kategori ismi en fazla 50 karakter içermelidir.");
        }
    }
}
