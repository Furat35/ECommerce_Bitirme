using ECommerce.Core.Filters;

namespace ECommerce.Business.Helpers.FilterServices.Orders
{
    public class OrderResponse<T> : ResponseFilter<T> where T : class, new()
    {
    }
}
