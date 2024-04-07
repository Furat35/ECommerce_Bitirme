using ECommerce.Core.Filters;

namespace ECommerce.Business.Helpers.FilterServices.OrderItems
{
    public class OrderItemResponse<T> : ResponseFilter<T> where T : class, new()
    {
    }
}
