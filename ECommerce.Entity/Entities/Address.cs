using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class Address : BaseEntity
    {
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public Guid DistrictId { get; set; }
        public District District { get; set; }
        public User User { get; set; }
    }
}
