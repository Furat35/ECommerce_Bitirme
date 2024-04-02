using ECommerce.Business.Models.Dtos.Orders;

namespace ECommerce.Business.Services.Contracts.IWriteServices
{
    public interface IOrderWriteService
    {
        Task CheckoutOrder(OrderCheckoutDto orderCheckoutDto);
        Task<bool> AddOrderStatus(Entity.Enums.OrderStatus orderStatus, string orderId);
    }
}
