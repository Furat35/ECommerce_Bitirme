using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class InvoiceInfo : BaseEntity
    {
        public string ClientFullName { get; set; }
        public string ClientPhone { get; set; }
        public float TotalPrice { get; set; }
        public Order Order { get; set; }
    }
}
