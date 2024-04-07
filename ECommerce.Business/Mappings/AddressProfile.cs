using AutoMapper;
using ECommerce.Business.Models.Dtos.Addresses;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressUpdateDto, Address>();
            CreateMap<Address, ShippingPlace>();
            CreateMap<Address, AddressListDto>();
            CreateMap<AddressListDto, ShippingPlace>();
        }
    }
}
