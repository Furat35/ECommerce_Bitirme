using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class OrderPaymentDetail : BaseEntity
    {
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public DateTime ExpireDate { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
