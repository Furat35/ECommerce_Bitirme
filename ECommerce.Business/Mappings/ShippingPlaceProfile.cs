using AutoMapper;
using ECommerce.Business.Models.Dtos.ShippingPlaces;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class ShippingPlaceProfile : Profile
    {
        public ShippingPlaceProfile()
        {
            CreateMap<ShippingPlaceCheckoutDto, ShippingPlace>();
            CreateMap<ShippingPlace, ShippingPlaceListDto>();
        }
    }
}
