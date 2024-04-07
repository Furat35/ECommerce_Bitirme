using AutoMapper;
using ECommerce.Business.Extensions;
using ECommerce.Business.Helpers.FilterServices;
using ECommerce.Business.Helpers.FilterServices.OrderItems;
using ECommerce.Business.Helpers.HeaderServices;
using ECommerce.Business.Models.Dtos.OrderItems;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Business.Services.ReadServices
{
    public class OrderItemReadService : IOrderItemReadService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        public OrderItemReadService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            OrderItems = unitOfWork.GetReadRepository<OrderItem>();
            _mapper = mapper;
            _httpContext = httpContextAccessor;
        }

        public IReadRepository<OrderItem> OrderItems { get; }

        /// <summary>
        /// Orders with given status for the company
        /// </summary>
        /// <param name="userId">Order Items that belong to the given user id of a company and has the given status</param>
        public async Task<List<OrderItemListDto>> GetOrderItemsWithGivenStatus(OrderItemRequestFilter filters, string userId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(userId);
            var orderItems = await OrderItems.GetWhere(_ => _.OrderItemStatuses.Any(_ => _.Status == filters.Status) && _.Product.CreatedBy == Guid.Parse(userId), false, [_ => _.OrderItemStatuses, _ => _.Product]).ToListAsync();
            var orderItemsWithGivenStatus = orderItems.Where(_ => _.OrderItemStatuses.Any(_ => _.Status == filters.Status && _.IsValid)).ToList();

            var filteredOrderItems = new OrderItemFilterService(_mapper, orderItemsWithGivenStatus).FilterOrderItems(filters);
            new HeaderService(_httpContext).AddToHeaders(filteredOrderItems.Headers);

            return filteredOrderItems.ResponseValue;
        }

        public async Task<bool> CheckIfOrderItemBelongsToGivenUser(string orderItemId, string userId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(orderItemId, userId);
            var orderItem = await OrderItems.GetByIdAsync(orderItemId, false, [_ => _.Product]);
            if (orderItem is null)
                throw new NotFoundException("Ürün bulunamadı!");

            return orderItem.Product.CreatedBy == Guid.Parse(userId);
        }
    }
}
