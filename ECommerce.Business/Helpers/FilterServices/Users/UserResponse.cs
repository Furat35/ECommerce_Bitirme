using ECommerce.Core.Filters;

namespace ECommerce.Business.Helpers.Users
{
    public class UserResponse<T> : ResponseFilter<T> where T : class, new()
    {
    }
}
