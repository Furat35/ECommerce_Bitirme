using AutoMapper;
using ECommerce.Business.Models.Dtos.Cities;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityListDto>();
        }
    }
}
