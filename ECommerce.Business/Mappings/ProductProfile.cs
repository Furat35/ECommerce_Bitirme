using AutoMapper;
using ECommerce.Business.Models.Dtos.Products;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductAddDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<Product, ProductListDto>();
            CreateMap<ProductListDto, OrderItemProduct>().ForMember(_ => _.Id, opt => opt.Ignore());
        }
    }
}
