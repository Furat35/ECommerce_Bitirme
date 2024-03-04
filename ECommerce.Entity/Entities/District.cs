using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class District : BaseEntity
    {
        public string DistrictName { get; set; }
        public Guid CityId { get; set; }
        public City City { get; set; }
        public virtual ICollection<Address>? Addresses { get; set; }
    }
}
