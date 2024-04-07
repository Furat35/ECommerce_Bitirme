using ECommerce.Business.Models.Dtos.Orders;
using FluentValidation;

namespace ECommerce.Business.Validations.FluentValidations.Orders
{
    public class OrderCheckoutDtoValidator : AbstractValidator<OrderCheckoutDto>
    {
        public OrderCheckoutDtoValidator()
        {
            //RuleFor(_ => _.)
        }
    }
}
