using AutoMapper;
using ECommerce.Business.Models.Dtos.SubCategories;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class SubCategoryProfile : Profile
    {
        public SubCategoryProfile()
        {
            CreateMap<SubCategory, SubCategoryListDto>();
            CreateMap<SubCategoryAddDto, SubCategory>();
            CreateMap<SubCategoryUpdateDto, SubCategory>();
        }
    }
}
