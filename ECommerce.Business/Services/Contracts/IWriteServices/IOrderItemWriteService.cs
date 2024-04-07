namespace ECommerce.Business.Services.Contracts.IWriteServices
{
    public interface IOrderItemWriteService
    {
        Task<bool> SetOrderItemStatus(string orderItemId, Entity.Enums.OrderItemStatus orderItemStatus);
    }
}
