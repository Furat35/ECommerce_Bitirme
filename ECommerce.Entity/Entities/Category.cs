using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<SubCategory>? SubCategories { get; set; }
    }
}
