using Microsoft.AspNetCore.Http;

namespace ECommerce.Business.Helpers.HeaderServices
{
    public class HeaderService
    {
        private readonly IHttpContextAccessor _httpContext;

        public HeaderService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public void AddToHeaders(Dictionary<string, string> headers)
        {
            foreach (var header in headers)
                _httpContext.HttpContext?.Response.Headers.Add(header.Key, header.Value);
        }
    }
}
