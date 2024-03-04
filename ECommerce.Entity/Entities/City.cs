using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class City : BaseEntity
    {
        public string CityName { get; set; }
        public Guid CountryId { get; set; }
        public Country Country { get; set; }
        public virtual ICollection<District>? Districts { get; set; }
    }
}
