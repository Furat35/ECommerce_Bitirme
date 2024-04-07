using AutoMapper;
using ECommerce.Business.Extensions;
using ECommerce.Business.Helpers.FilterServices;
using ECommerce.Business.Helpers.FilterServices.Orders;
using ECommerce.Business.Helpers.HeaderServices;
using ECommerce.Business.Models.Dtos.Orders;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Business.Services.ReadServices
{
    public class OrderReadService : IOrderReadService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public OrderReadService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            Orders = unitOfWork.GetReadRepository<Order>();
            OrderItems = unitOfWork.GetReadRepository<OrderItem>();
            _mapper = mapper;
            _httpContext = contextAccessor;
        }

        public IReadRepository<Order> Orders { get; }
        public IReadRepository<OrderItem> OrderItems { get; }

        public async Task<OrderListDto> GetOrderById(string orderId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(orderId);
            var order = await Orders
                         .GetWhere(_ => _.Id == Guid.Parse(orderId), false,
                         [_ => _.ShippingPlace, _ => _.InvoiceInfo, _ => _.OrderPaymentDetail, _ => _.User, _ => _.OrderStatuses])
                         .Include(_ => _.OrderItems)
                             .ThenInclude(oi => oi.OrderItemStatuses)
                         .Include(_ => _.OrderItems)
                             .ThenInclude(oi => oi.Product)
                         .FirstOrDefaultAsync();
            if (order is null)
                throw new NotFoundException("Sipariş bulunamadı!");

            return _mapper.Map<OrderListDto>(order);
        }

        // todo: performance can be improved!!
        public async Task<List<OrderListDto>> GetOrdersWithGivenStatus(OrderRequestFilter filters, string userId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(userId);
            var orders = await Orders
                .GetWhere(_ => _.UserId == Guid.Parse(userId), false,
                [_ => _.ShippingPlace, _ => _.InvoiceInfo, _ => _.OrderPaymentDetail, _ => _.User, _ => _.OrderStatuses])
                .Include(_ => _.OrderItems)
                    .ThenInclude(oi => oi.OrderItemStatuses)
                .Include(_ => _.OrderItems)
                    .ThenInclude(oi => oi.Product).ToListAsync();

            var notCompletedOrders = orders.Where(order =>
            {
                bool isNotCompleted = order.OrderStatuses.Any(os => os.Status == filters.Status && os.IsValid);
                return isNotCompleted;
            }).ToList();

            var filteredOrders = new OrderFilterService(_mapper, notCompletedOrders).FilterOrders(filters);
            new HeaderService(_httpContext).AddToHeaders(filteredOrders.Headers);

            return filteredOrders.ResponseValue;
        }

        public async Task<bool> CheckIfOrderIsCreatedByGivenUser(string orderId, string userId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(orderId, userId);
            var order = await Orders.GetByIdAsync(orderId);
            return order.UserId == Guid.Parse(userId) ? true : false;
        }
    }
}
