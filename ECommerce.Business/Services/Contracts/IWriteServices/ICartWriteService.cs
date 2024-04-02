using ECommerce.Business.Models.Dtos.CartItems;

namespace ECommerce.Business.Services.Contracts.IWriteServices
{
    public interface ICartWriteService
    {
        Task<bool> CreateCartAsync(string userId);
        Task<bool> AddItemToCart(CartItemAddDto cartItemAddDto, string userId);
        Task<bool> RemoveItemFromCart(string productId, string userId);
        Task<bool> DecreaseItemQuantity(string productId, int quantity, string userId);
        Task<bool> ClearCart(string userId);
    }
}
