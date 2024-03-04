using ECommerce.Business.Models.Dtos.CartItems;
using FluentValidation;

namespace ECommerce.Business.Validations.FluentValidations.CartItems
{
    public class CartItemAddDtoValidator : AbstractValidator<CartItemAddDto>
    {
        public CartItemAddDtoValidator()
        {
            RuleFor(_ => _.ProductId)
               .NotNull()
               .WithMessage("Ürün id'si boş olamaz.")
               .NotEmpty()
               .WithMessage("Ürün id'si boş olamaz.");

            RuleFor(_ => _.Quantity)
               .NotNull()
               .WithMessage("Ürün adeti boş olamaz.")
               .NotEmpty()
               .WithMessage("Ürün adeti boş olamaz.")
               .GreaterThan(0)
               .WithMessage("Ürün adeti 0'dan büyük olmalıdır.");
        }
    }
}
