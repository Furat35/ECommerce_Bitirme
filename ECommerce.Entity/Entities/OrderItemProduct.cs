using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class OrderItemProduct : BaseEntity
    {
        public string ProductName { get; set; }
        public string SubProductName { get; set; }
        public string ProductDescription { get; set; }
        public float Price { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid SubCategoryId { get; set; }
        public Guid BrandId { get; set; }
        public Guid OrderItemId { get; set; }
    }
}
