using ECommerce.Business.Models.Dtos.Products;

namespace ECommerce.Business.Models.Dtos.CartItems
{
    public class CartItemListDto
    {
        public int Quantity { get; set; }
        public ProductListDto Product { get; set; }
    }
}
