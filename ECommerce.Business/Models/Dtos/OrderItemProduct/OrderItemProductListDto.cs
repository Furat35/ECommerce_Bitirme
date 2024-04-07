namespace ECommerce.Business.Models.Dtos.OrderItemProduct
{
    public class OrderItemProductListDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string SubProductName { get; set; }
        public string ProductDescription { get; set; }
        public float Price { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid SubCategoryId { get; set; }
        public Guid BrandId { get; set; }
    }
}
