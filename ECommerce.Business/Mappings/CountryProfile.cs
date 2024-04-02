using AutoMapper;
using ECommerce.Business.Models.Dtos.Countries;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<Country, CountryListDto>();
        }
    }
}
