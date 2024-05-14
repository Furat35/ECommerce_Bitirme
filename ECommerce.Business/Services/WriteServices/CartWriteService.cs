using ECommerce.Business.Extensions;
using ECommerce.Business.Models.Dtos.CartItems;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using FluentValidation;

namespace ECommerce.Business.Services.WriteServices
{
    public class CartWriteService : ICartWriteService
    {
        private readonly IWriteRepository<Cart> _cartWriteRepository;
        private readonly IProductReadService _productReadService;
        private readonly ICartReadService _cartReadService;
        private readonly IUserReadService _userReadService;
        private readonly IValidator<CartItemAddDto> _cartItemAddDtoValidator;

        public CartWriteService(IUnitOfWork unitOfWork, ICartReadService cartReadService, IProductReadService productReadService,
            IUserReadService userReadService, IValidator<CartItemAddDto> cartItemAddDtoValidator)
        {
            _cartWriteRepository = unitOfWork.GetWriteRepository<Cart>();
            _cartReadService = cartReadService;
            _productReadService = productReadService;
            _userReadService = userReadService;
            _cartItemAddDtoValidator = cartItemAddDtoValidator;
        }

        public async Task<bool> AddItemToCart(CartItemAddDto cartItemAddDto, string userId)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(cartItemAddDto, _cartItemAddDtoValidator);
            var cart = await _cartReadService.Carts.GetSingleAsync(_ => _.Id.ToString() == userId, includeProperties: _ => _.CartItems);
            var cartItems = cart.CartItems.FirstOrDefault(_ => _.ProductId.ToString().Equals(cartItemAddDto.ProductId, StringComparison.InvariantCultureIgnoreCase));
            if (cartItems != null)
                cartItems.Quantity += cartItemAddDto.Quantity;
            else
            {
                var product = await _productReadService.GetProductByIdAsync(cartItemAddDto.ProductId);
                var newCartItem = new CartItem
                {
                    ProductId = product.Id,
                    Quantity = cartItemAddDto.Quantity,
                    IsValid = true
                };
                cart.CartItems.Add(newCartItem);
            }

            return await _cartWriteRepository.UpdateAsync(cart);
        }

        public async Task<bool> RemoveItemFromCart(string productId, string userId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(productId, userId);
            var cart = await _cartReadService.Carts.GetSingleAsync(_ => _.Id.ToString() == userId, includeProperties: _ => _.CartItems);
            var cartItem = cart.CartItems.FirstOrDefault(_ => _.ProductId.ToString().Equals(productId, StringComparison.InvariantCultureIgnoreCase));
            if (cartItem is null)
                return false;
            cart.CartItems.Remove(cartItem);

            return await _cartWriteRepository.UpdateAsync(cart);
        }

        public async Task<bool> DecreaseItemQuantity(string productId, int quantity, string userId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(productId, userId);
            if (quantity <= 0)
                throw new BadRequestException("Geçersiz miktar girildi! Tekrar giriniz!");

            var cart = await _cartReadService.Carts.GetSingleAsync(_ => _.Id.ToString() == userId, includeProperties: _ => _.CartItems);
            var cartItem = cart.CartItems.FirstOrDefault(_ => _.ProductId.ToString().Equals(productId, StringComparison.InvariantCultureIgnoreCase));
            if (cartItem is null)
                return false;
            cartItem.Quantity = quantity;

            return await _cartWriteRepository.UpdateAsync(cart);
        }


        public async Task<bool> ClearCart(string userId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(userId);
            var cart = await _cartReadService.Carts.GetSingleAsync(_ => _.Id.ToString() == userId, includeProperties: _ => _.CartItems);
            if (cart is null)
                return false;
            cart.CartItems.Clear();

            return await _cartWriteRepository.UpdateAsync(cart);
        }

        public async Task<bool> CreateCartAsync(string userId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(userId);
            var cart = await _cartReadService.Carts.GetSingleAsync(_ => _.Id.ToString().Equals(userId, StringComparison.InvariantCultureIgnoreCase));
            if (cart != null)
                throw new BadRequestException("Sepet önceden oluşturulmuştu!");

            var user = await _userReadService.Users.GetByIdAsync(userId);
            cart = new Cart
            {
                User = user
            };

            return await _cartWriteRepository.AddAsync(cart);
        }
    }
}
