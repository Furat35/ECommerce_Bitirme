using AutoMapper;
using ECommerce.Business.Models.Dtos.OrderItems;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItemCheckoutDto, OrderItem>();
            CreateMap<OrderItem, OrderItemListDto>();
        }
    }
}
