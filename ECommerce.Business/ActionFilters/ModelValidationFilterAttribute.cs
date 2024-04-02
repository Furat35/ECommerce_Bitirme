using ECommerce.Core.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerce.Business.ActionFilters
{
    public class ModelValidationFilterAttribute : IActionFilter
    {
        private readonly string[] _keys;
        public ModelValidationFilterAttribute(string[] keys)
        {
            _keys = keys;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            ModelValidation(context);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private void ModelValidation(ActionExecutingContext context)
        {
            QueryAndRouteParameterValidation(context);
            BodyArgumentValidation(context);
        }

        public void BodyArgumentValidation(ActionExecutingContext context)
        {
            foreach (var key in _keys)
            {
                var body = context.ActionArguments.FirstOrDefault(context => context.Key == key);
                if (body.Key != null && body.Value == null)
                    throw new BadRequestException($"{body.Key} boş olamaz!");
            }
        }

        public void QueryAndRouteParameterValidation(ActionExecutingContext context)
        {
            foreach (var key in _keys)
            {
                var parameter = context.ModelState.FirstOrDefault(_ => _.Key == key);
                if (parameter.Value == null || parameter.Key != null && parameter.Value?.ValidationState != Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
                    throw new BadRequestException($"{_keys} boş olamaz!");
            }
        }
    }
}
