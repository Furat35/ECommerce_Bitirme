using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class Country : BaseEntity
    {
        public string CountryName { get; set; }
        public virtual ICollection<City>? Cities { get; set; }
    }
}
