using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class SubCategory : BaseEntity
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
