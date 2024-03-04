using ECommerce.Business.Models.Dtos.CartItems;
using ECommerce.Business.Services.Carts.Abstract;
using ECommerce.Business.Services.Products.Abstract;
using ECommerce.Business.Services.Users.Abstract;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Services.Carts
{
    public class CartWriteService : ICartWriteService
    {
        private readonly IWriteRepository<Cart> _cartWriteRepository;
        private readonly IProductReadService _productReadService;
        private readonly ICartReadService _cartReadService;
        private readonly IUserReadService _userReadService;

        public CartWriteService(IUnitOfWork unitOfWork, ICartReadService cartReadService, IProductReadService productReadService,
            IUserReadService userReadService)
        {
            _cartWriteRepository = unitOfWork.GetWriteRepository<Cart>();
            _cartReadService = cartReadService;
            _productReadService = productReadService;
            _userReadService = userReadService;
        }

        public async Task<bool> AddItemToCart(CartItemAddDto cartItemAddDto, string userId)
        {
            var cart = await _cartReadService.Carts.GetSingleAsync(_ => _.UserId.ToString() == userId, includeProperties: _ => _.CartItems);
            var cartItems = cart.CartItems.FirstOrDefault(_ => _.ProductId.ToString().Equals(cartItemAddDto.ProductId, StringComparison.InvariantCultureIgnoreCase));
            if (cartItems != null)
                cartItems.Quantity += cartItemAddDto.Quantity;
            else
            {
                var product = await _productReadService.GetProductIdAsync(cartItemAddDto.ProductId);
                var newCartItem = new CartItem
                {
                    ProductId = product.Id,
                    Quantity = cartItemAddDto.Quantity,
                };
                cart.CartItems.Add(newCartItem);
            }

            return await SaveChangesAsync();
        }

        public async Task<bool> ClearCart(string userId)
        {
            var cart = await _cartReadService.Carts.GetSingleAsync(_ => _.UserId.ToString() == userId, includeProperties: _ => _.CartItems);
            if (cart is null)
                return false;
            cart.CartItems.Clear();
            _cartWriteRepository.Update(cart);

            return await SaveChangesAsync();
        }

        public async Task<bool> CreateCartAsync(string userId)
        {
            var cart = await _cartReadService.Carts.GetSingleAsync(_ => _.UserId.ToString().Equals(userId, StringComparison.InvariantCultureIgnoreCase));
            if (cart != null)
                throw new BadRequestException("Sepet oluşturulmuştu!");

            var user = await _userReadService.Users.GetByIdAsync(userId);
            cart = new Cart
            {
                User = user
            };
            await _cartWriteRepository.AddAsync(cart);

            return await SaveChangesAsync();
        }

        private async Task<bool> SaveChangesAsync()
          => await _cartWriteRepository.SaveAsync() != 0;
    }
}
