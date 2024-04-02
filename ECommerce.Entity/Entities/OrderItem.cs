using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class OrderItem : BaseEntity
    {
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public virtual ICollection<OrderItemStatus> OrderItemStatuses { get; set; } = new List<OrderItemStatus>();
        public OrderItemProduct Product { get; set; }
    }
}
