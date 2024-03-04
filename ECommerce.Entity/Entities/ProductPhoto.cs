using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class ProductPhoto : BaseEntity
    {
        public string PhotoPath { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
