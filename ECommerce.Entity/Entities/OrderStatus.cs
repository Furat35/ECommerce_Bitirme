using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class OrderStatus : BaseEntity
    {
        public Enums.OrderStatus Status { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
