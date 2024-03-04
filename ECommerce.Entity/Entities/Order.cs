using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class Order : BaseEntity
    {
        public float TotalPrice { get; set; }
        public virtual ICollection<OrderStatus> OrderStatuses { get; set; }
        public Guid ShippingPlaceId { get; set; }
        public ShippingPlace ShippingPlace { get; set; }
        public Guid InvoiceInfoId { get; set; }
        public InvoiceInfo InvoiceInfo { get; set; }
        public Guid OrderPaymentDetailId { get; set; }
        public OrderPaymentDetail OrderPaymentDetail { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
