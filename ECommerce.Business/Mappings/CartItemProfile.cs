using AutoMapper;
using ECommerce.Business.Models.Dtos.CartItems;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItem, CartItemListDto>();
            CreateMap<CartItemListDto, OrderItem>();
        }
    }
}
