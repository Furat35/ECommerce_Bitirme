using ECommerce.Business.Models.Dtos.Products;
using FluentValidation;

namespace ECommerce.Business.Validations.FluentValidations.Products
{
    public class ProductAddDtoValidator : AbstractValidator<ProductAddDto>
    {
        public ProductAddDtoValidator()
        {
            RuleFor(_ => _.ProductName)
                .NotNull()
                .WithMessage("Ürün ismi boş olamaz.")
                .NotEmpty()
                .WithMessage("Ürün ismi boş olamaz.")
                .MinimumLength(2)
                .WithMessage("Ürün ismi en az 2 karakter içermelidir.")
                .MaximumLength(50)
                .WithMessage("Ürün ismi en fazle 50 karakter içermelidir.");

            RuleFor(_ => _.SubProductName)
               .NotNull()
               .WithMessage("Ürün alt kategorisi boş olamaz.")
               .NotEmpty()
               .WithMessage("Ürün alt kategorisi boş olamaz.")
               .MinimumLength(2)
               .WithMessage("Ürün alt kategorisi en az 2 karakter içermelidir.")
               .MaximumLength(50)
               .WithMessage("Ürün alt kategorisi en fazle 50 karakter içermelidir.");

            RuleFor(_ => _.ProductDescription)
               .NotNull()
               .WithMessage("Ürün açıklaması boş olamaz.")
               .NotEmpty()
               .WithMessage("Ürün açıklaması boş olamaz.")
               .MinimumLength(2)
               .WithMessage("Ürün açıklaması en az 2 karakter içermelidir.")
               .MaximumLength(200)
               .WithMessage("Ürün açıklaması en fazle 200 karakter içermelidir.");

            RuleFor(_ => _.Price)
                .NotNull()
                .WithMessage("Fiyat boş olamaz.")
                .NotEmpty()
                .WithMessage("Fiyat boş olamaz.")
                .GreaterThan(0)
                .WithMessage("Fiyat 0'dan büyük olmalıdır.")
                .LessThan(1000000)
                .WithMessage("Fiyat 1000000'den küçük olmalıdır.");

            RuleFor(_ => _.Stock)
              .NotNull()
              .WithMessage("Stok boş olamaz.")
              .NotEmpty()
              .WithMessage("Stok boş olamaz.")
              .GreaterThan(0)
              .WithMessage("Stok 0'dan büyük olmalıdır.")
              .LessThan(1000)
              .WithMessage("Stok 1000'den küçük olmalıdır.");

        }
    }
}
