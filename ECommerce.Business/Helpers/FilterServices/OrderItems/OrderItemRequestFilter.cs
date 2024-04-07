using ECommerce.Core.Filters;

namespace ECommerce.Business.Helpers.FilterServices.OrderItems
{
    public class OrderItemRequestFilter : Pagination
    {
        public Entity.Enums.OrderItemStatus Status { get; set; } = Entity.Enums.OrderItemStatus.Pending;
    }
}
