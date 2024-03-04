using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
