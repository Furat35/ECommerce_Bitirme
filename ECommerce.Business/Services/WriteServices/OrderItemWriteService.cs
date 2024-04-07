using ECommerce.Business.Extensions;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Services.WriteServices
{
    public class OrderItemWriteService : IOrderItemWriteService
    {
        private readonly IWriteRepository<OrderItem> _orderItemWriteRepository;
        private readonly IOrderItemReadService _orderReadService;

        public OrderItemWriteService(IUnitOfWork unitOfWork, IOrderItemReadService orderReadService)
        {
            _orderItemWriteRepository = unitOfWork.GetWriteRepository<OrderItem>();
            _orderReadService = orderReadService;
        }

        public async Task<bool> SetOrderItemStatus(string orderItemId, Entity.Enums.OrderItemStatus orderItemStatus)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(orderItemId);
            var orderItem = await _orderReadService.OrderItems.GetByIdAsync(orderItemId, false, [_ => _.OrderItemStatuses]);
            if (orderItem is null)
                throw new NotFoundException("Ürün bulunamadı!");

            foreach (var status in orderItem.OrderItemStatuses)
                status.IsValid = false;

            orderItem.OrderItemStatuses.Add(new OrderItemStatus() { Status = orderItemStatus, IsValid = true });

            return await _orderItemWriteRepository.UpdateAsync(orderItem);
        }
    }
}
