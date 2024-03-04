namespace ECommerce.Business.Models.Dtos.Products
{
    public class ProductAddDto
    {
        public string ProductName { get; set; }
        public string SubProductName { get; set; }
        public string ProductDescription { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
        public Guid SubCategoryId { get; set; }
        public Guid BrandId { get; set; }
    }
}
