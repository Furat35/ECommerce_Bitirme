using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class InvoiceInfo : BaseEntity
    {
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyPhone { get; set; }
        public string ClientFullName { get; set; }
        public string ClientPhone { get; set; }
        public float TotalPrice { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
