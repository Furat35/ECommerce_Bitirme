using AutoMapper;
using ECommerce.Business.Models.Dtos.Carts;
using ECommerce.Business.Services.Carts.Abstract;
using ECommerce.Business.Services.Products.Abstract;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Business.Services.Carts
{
    public class CartReadService : ICartReadService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IProductReadService _productReadService;

        public CartReadService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor, IProductReadService productReadService)
        {
            Carts = unitOfWork.GetReadRepository<Cart>();
            _mapper = mapper;
            _httpContext = contextAccessor;
            _productReadService = productReadService;
        }

        public IReadRepository<Cart> Carts { get; }

        public async Task<CartListDto> GetCartAsync(string userId)
        {
            var cart = await Carts.GetSingleAsync(_ => _.UserId.ToString() == userId, includeProperties: [_ => _.CartItems]);
            if (cart is null)
                throw new NotFoundException("Sepet bulunamadı!");
            foreach (var cartItem in cart.CartItems)
                cartItem.Product = await _productReadService.Products.GetByIdAsync(cartItem.ProductId.ToString(), includeProperties: [_ => _.SubCategory, _ => _.Brand]);
            var response = _mapper.Map<CartListDto>(cart);

            return response;
        }
    }
}
