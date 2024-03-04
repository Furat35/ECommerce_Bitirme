using AutoMapper;
using ECommerce.Business.Models.Dtos.Brands;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<Brand, BrandListDto>();
            CreateMap<BrandAddDto, Brand>();
            CreateMap<BrandUpdateDto, Brand>();
        }
    }
}
