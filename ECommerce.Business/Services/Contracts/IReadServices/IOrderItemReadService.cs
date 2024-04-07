using ECommerce.Business.Helpers.FilterServices.OrderItems;
using ECommerce.Business.Models.Dtos.OrderItems;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Services.Contracts.IReadServices
{
    public interface IOrderItemReadService
    {
        IReadRepository<OrderItem> OrderItems { get; }
        Task<List<OrderItemListDto>> GetOrderItemsWithGivenStatus(OrderItemRequestFilter filters, string userId);
        Task<bool> CheckIfOrderItemBelongsToGivenUser(string orderItemId, string userId);
    }
}
