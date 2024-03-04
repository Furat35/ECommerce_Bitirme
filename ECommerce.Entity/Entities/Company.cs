using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class Company : BaseEntity
    {
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string AboutCompany { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
    }
}
