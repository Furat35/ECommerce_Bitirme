using AutoMapper;
using ECommerce.Business.Models.Dtos.Categories;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryListDto>();
            CreateMap<CategoryAddDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
        }
    }
}
