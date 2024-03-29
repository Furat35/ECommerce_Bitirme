﻿using ECommerce.Business.Models.Dtos.Categories;
using FluentValidation;

namespace ECommerce.Business.Validations.FluentValidations.Categories
{
    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateDtoValidator()
        {
            RuleFor(_ => _.Id)
               .NotEmpty()
               .WithMessage("Id boş olamaz.");

            RuleFor(_ => _.Name)
               .NotEmpty()
               .WithMessage("Kategory ismi boş olamaz.")
               .MinimumLength(2)
               .WithMessage("Kategory ismi en az 2 karakter içermelidir.")
               .MaximumLength(50)
               .WithMessage("Kategory ismi en fazle 50 karakter içermelidir.");
        }
    }
}
