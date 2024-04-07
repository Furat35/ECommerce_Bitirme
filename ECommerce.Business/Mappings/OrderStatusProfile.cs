using AutoMapper;
using ECommerce.Business.Models.Dtos.OrderStatuses;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class OrderStatusProfile : Profile
    {
        public OrderStatusProfile()
        {
            CreateMap<OrderStatus, OrderStatusListDto>();
        }
    }
}
