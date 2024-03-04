using AutoMapper;
using ECommerce.Business.Models.Dtos.Carts;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartListDto>();
        }
    }
}
