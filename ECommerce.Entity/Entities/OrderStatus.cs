using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class OrderStatus : BaseEntity
    {
        public ECommerce.Entity.Enums.OrderStatus Status { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
