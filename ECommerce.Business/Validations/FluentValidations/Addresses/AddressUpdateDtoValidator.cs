using ECommerce.Business.Models.Dtos.Addresses;
using FluentValidation;

namespace ECommerce.Business.Validations.FluentValidations.Addresses
{
    public class AddressUpdateDtoValidator : AbstractValidator<AddressUpdateDto>
    {
        public AddressUpdateDtoValidator()
        {
            RuleFor(_ => _.Neighborhood)
                .NotEmpty()
                .WithMessage("Mahalle adı boş olamaz.")
                .MinimumLength(2)
                .WithMessage("Mahalle adı en az 2 karakter içermelidir.")
                .MaximumLength(50)
                .WithMessage("Mahalle adı en fazle 50 karakter içermelidir.");

            RuleFor(_ => _.Street)
                .NotEmpty()
                .WithMessage("Sokak adı boş olamaz.")
                .MinimumLength(2)
                .WithMessage("Sokak adı en az 2 karakter içermelidir.")
                .MaximumLength(50)
                .WithMessage("Sokak adı en fazle 50 karakter içermelidir.");

            RuleFor(_ => _.Address1)
                .NotEmpty()
                .WithMessage("Adres alanı boş olamaz.")
                .MinimumLength(2)
                .WithMessage("Adres alanı en az 2 karakter içermelidir.")
                .MaximumLength(50)
                .WithMessage("Adres alanı en fazle 50 karakter içermelidir.");

            RuleFor(_ => _.DistrictId)
                .Custom((param, context) =>
                {
                    if (!Guid.TryParse(param.ToString(), out Guid result))
                    {
                        context.AddFailure("Geçersiz ilçe id'si.");
                    }
                });
        }
    }
}
