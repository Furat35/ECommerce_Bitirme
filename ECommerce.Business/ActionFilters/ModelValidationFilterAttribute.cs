using ECommerce.Core.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerce.Business.ActionFilters
{
    public class ModelValidationFilterAttribute : IActionFilter
    {
        private readonly string _key;
        public ModelValidationFilterAttribute(string key)
        {
            _key = key;
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
            var body = context.ActionArguments.FirstOrDefault(context => context.Key == _key);
            if (body.Key != null && body.Value == null)
                throw new BadRequestException($"{body.Key} boş olamaz!");
        }

        public void QueryAndRouteParameterValidation(ActionExecutingContext context)
        {
            var parameter = context.ModelState.FirstOrDefault(_ => _.Key == _key);
            if (parameter.Value == null || parameter.Key != null && parameter.Value?.ValidationState != Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
                throw new BadRequestException($"{_key} boş olamaz!");
        }
    }
}
