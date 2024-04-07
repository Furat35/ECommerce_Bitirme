using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class OrderItemStatus : BaseEntity
    {
        public Enums.OrderItemStatus Status { get; set; }
        public Guid OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; }
    }
}
