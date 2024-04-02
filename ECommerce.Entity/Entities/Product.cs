using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class Product : BaseEntity
    {
        public string ProductName { get; set; }
        public string SubProductName { get; set; }
        public string ProductDescription { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
        public Guid CreatedBy { get; set; }
        //public User User { get; set; }
        public Guid SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }
        public virtual ICollection<Feedback>? Feedbacks { get; set; } = new List<Feedback>();
        public virtual ICollection<ProductPhoto>? ProductPhotos { get; set; } = new List<ProductPhoto>();
    }
}
