using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class Cart : BaseEntity
    {
        public Cart()
        {
            CartItems = new List<CartItem>();
        }

        public virtual ICollection<CartItem> CartItems { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
