using ECommerce.Business.Models.Dtos.CartItems;

namespace ECommerce.Business.Models.Dtos.Carts
{
    public class CartListDto
    {
        public string Id { get; set; }
        public float TotalPrice { get; set; }
        public ICollection<CartItemListDto>? CartItems { get; set; }
    }
}
