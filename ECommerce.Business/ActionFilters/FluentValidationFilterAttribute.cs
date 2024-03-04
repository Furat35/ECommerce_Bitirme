using ECommerce.Core.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerce.Business.ActionFilters
{
    public class FluentValidationFilterAttribute<TValidator, T> : IActionFilter
        where TValidator : IValidator<T>, new()
        where T : class, new()
    {
        private readonly string _key;

        public FluentValidationFilterAttribute(string key)
        {
            _key = key;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            FluentValidation(context);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private void FluentValidation(ActionExecutingContext context)
        {
            var argumentValue = context.ActionArguments.FirstOrDefault(_ => _.Key == _key).Value ?? null;
            if (argumentValue != null)
            {
                TValidator validator = new TValidator();
                var validationResult = validator.Validate((T)argumentValue);
                if (!validationResult.IsValid)
                    throw new BadRequestException(validationResult.Errors.First().ErrorMessage);
            }
            else
                throw new BadRequestException("Tüm alanları doldurunuz!");
        }
    }
}
