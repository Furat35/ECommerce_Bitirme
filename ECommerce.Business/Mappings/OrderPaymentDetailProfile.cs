using AutoMapper;
using ECommerce.Business.Models.Dtos.OrderPaymentDetails;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class OrderPaymentDetailProfile : Profile
    {
        public OrderPaymentDetailProfile()
        {
            CreateMap<OrderPaymentDetailCheckoutDto, OrderPaymentDetail>();
            CreateMap<OrderPaymentDetail, OrderPaymentDetailListDto>();
        }
    }
}
