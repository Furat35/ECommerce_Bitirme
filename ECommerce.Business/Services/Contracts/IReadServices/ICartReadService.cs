using ECommerce.Business.Models.Dtos.Carts;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Services.Contracts.IReadServices
{
    public interface ICartReadService
    {
        Task<CartListDto> GetCartAsync(string userId);
        IReadRepository<Cart> Carts { get; }
    }
}
