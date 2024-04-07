using AutoMapper;
using ECommerce.Business.Models.Dtos.Orders;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderCheckoutDto, Order>();
            CreateMap<Order, OrderListDto>();
        }
    }
}
