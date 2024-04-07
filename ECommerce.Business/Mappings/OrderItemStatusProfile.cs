using AutoMapper;
using ECommerce.Business.Models.Dtos.OrderItemStatuses;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class OrderItemStatusProfile : Profile
    {
        public OrderItemStatusProfile()
        {
            CreateMap<OrderItemStatus, OrderItemStatusListDto>();
        }
    }
}
