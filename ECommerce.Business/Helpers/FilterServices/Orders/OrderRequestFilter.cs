using ECommerce.Core.Filters;

namespace ECommerce.Business.Helpers.FilterServices.Orders
{
    public class OrderRequestFilter : Pagination
    {
        public Entity.Enums.OrderStatus Status { get; set; } = Entity.Enums.OrderStatus.Pending;
    }
}
