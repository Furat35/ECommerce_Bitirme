using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class PaymentCard : BaseEntity
    {
        public string NameSurname { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public DateTime ExpireDate { get; set; }
        public User User { get; set; }
    }
}
