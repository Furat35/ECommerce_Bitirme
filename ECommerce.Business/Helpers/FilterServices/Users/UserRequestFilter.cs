using ECommerce.Core.Filters;

namespace ECommerce.Business.Helpers.Users
{
    public class UserRequestFilter : Pagination
    {
        public string? Name { get; set; }
        public string? Mail { get; set; }
    }
}
