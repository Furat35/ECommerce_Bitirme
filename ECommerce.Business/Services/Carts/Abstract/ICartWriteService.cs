using ECommerce.Business.Models.Dtos.CartItems;

namespace ECommerce.Business.Services.Carts.Abstract
{
    public interface ICartWriteService
    {
        Task<bool> CreateCartAsync(string userId);
        Task<bool> AddItemToCart(CartItemAddDto cartItemAddDto, string userId);
        Task<bool> ClearCart(string userId);
    }
}
