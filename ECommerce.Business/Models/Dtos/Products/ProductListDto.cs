using ECommerce.Business.Models.Dtos.Brands;
using ECommerce.Business.Models.Dtos.SubCategories;

namespace ECommerce.Business.Models.Dtos.Products
{
    public class ProductListDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string SubProductName { get; set; }
        public string ProductDescription { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
        public SubCategoryListDto SubCategory { get; set; }
        public BrandListDto Brand { get; set; }
    }
}
