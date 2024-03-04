using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class Feedback : BaseEntity
    {
        public string UserFeedback { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
