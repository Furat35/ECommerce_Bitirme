using AutoMapper;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
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
            _mapper = mapper;
            _httpContext = contextAccessor;
        }

        public IReadRepository<Order> Orders { get; }

        public async Task<List<Order>> GetCompletedOrders(string userId)
        {
            var orders = Orders
                .GetWhere(_ => _.OrderStatuses.FirstOrDefault(_ => _.IsValid).Status == ECommerce.Entity.Enums.OrderStatus.Completed &&
                               _.UserId == Guid.Parse(userId), false, [_ => _.OrderStatuses]);

            return await orders.ToListAsync();
        }
    }
}
