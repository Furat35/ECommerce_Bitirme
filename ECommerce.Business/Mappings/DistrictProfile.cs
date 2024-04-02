using AutoMapper;
using ECommerce.Business.Models.Dtos.Districts;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class DistrictProfile : Profile
    {
        public DistrictProfile()
        {
            CreateMap<District, DistrictListDto>();
        }
    }
}
