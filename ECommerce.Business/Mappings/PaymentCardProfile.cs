using AutoMapper;
using ECommerce.Business.Models.Dtos.PaymentCards;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class PaymentCardProfile : Profile
    {
        public PaymentCardProfile()
        {
            CreateMap<PaymentCard, PaymentCardListDto>();
            CreateMap<PaymentCardAddDto, PaymentCard>().ReverseMap();
            CreateMap<PaymentCard, PaymentCardUpdateDto>().ReverseMap();
            CreateMap<PaymentCardAddDto, PaymentCardUpdateDto>();
        }
    }
}
