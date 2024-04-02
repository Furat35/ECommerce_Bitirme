using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class Order : BaseEntity
    {
        public float TotalPrice { get; set; }
        public ShippingPlace ShippingPlace { get; set; }
        public InvoiceInfo InvoiceInfo { get; set; }
        public OrderPaymentDetail OrderPaymentDetail { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public virtual ICollection<OrderStatus> OrderStatuses { get; set; } = new List<OrderStatus>();
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
