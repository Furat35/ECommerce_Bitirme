using AutoMapper;
using ECommerce.Business.Models.Dtos.OrderItemProduct;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class OrderItemProductProfile : Profile
    {
        public OrderItemProductProfile()
        {
            CreateMap<OrderItemProduct, OrderItemProductListDto>();
        }
    }
}
