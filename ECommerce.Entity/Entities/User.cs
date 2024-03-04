using ECommerce.Core.Entities;
using ECommerce.Entity.Enums;

namespace ECommerce.Entity.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public Role Role { get; set; }
        public Guid? CartId { get; set; }
        public Cart Cart { get; set; }
        public Guid? CompanyId { get; set; }
        public Company Company { get; set; }
        public Guid? AddressId { get; set; }
        public Address Address { get; set; }
        public Guid? PaymentCardId { get; set; }
        public PaymentCard PaymentCard { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
