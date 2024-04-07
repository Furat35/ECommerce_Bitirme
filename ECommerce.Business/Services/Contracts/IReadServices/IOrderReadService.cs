using ECommerce.Business.Helpers.FilterServices.Orders;
using ECommerce.Business.Models.Dtos.Orders;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Services.Contracts.IReadServices
{
    public interface IOrderReadService
    {
        IReadRepository<Order> Orders { get; }
        Task<List<OrderListDto>> GetOrdersWithGivenStatus(OrderRequestFilter filters, string userId);
        Task<OrderListDto> GetOrderById(string orderId);
        Task<bool> CheckIfOrderIsCreatedByGivenUser(string orderId, string userId);
    }
}
