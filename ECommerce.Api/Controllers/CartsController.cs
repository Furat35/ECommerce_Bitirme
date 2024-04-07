using ECommerce.Business.Models.Dtos.CartItems;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Core.Consts;
using ECommerce.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    /// <summary>
    /// Kullanıcı sepetiyle ilgili crud işlemlerinin yapıldığı endpointleri içermektedir.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartReadService _cartReadService;
        private readonly ICartWriteService _cartWriteService;

        public CartsController(ICartReadService cartReadService, ICartWriteService cartWriteService)
        {
            _cartReadService = cartReadService;
            _cartWriteService = cartWriteService;
        }

        /// <summary>
        /// Kullanıcı sepeti getiriliyor
        /// </summary>
        /// <returns>Sepet getirilmektedir</returns>
        [HttpGet(Name = "GetCart")]
        [Authorize(Roles = $"{RoleConsts.User}")]
        public async Task<IActionResult> GetCart()
        {
            var carts = await _cartReadService.GetCartAsync(HttpContext.User.GetActiveUserId());
            return Ok(carts);
        }

        /// <summary>
        /// Sepete ürün ekleme
        /// </summary>
        /// <param name="cartItem">Sepete eklenecek ürün detayları</param>
        /// <returns>Eklenen ürün</returns>
        [HttpPut(Name = "UpdateCart")]
        [Authorize(Roles = $"{RoleConsts.User}")]
        public async Task<IActionResult> UpdateCart([FromBody] CartItemAddDto cartItem)
        {
            bool isAdded = await _cartWriteService.AddItemToCart(cartItem, HttpContext.User.GetActiveUserId());
            return Ok(isAdded);
        }

        /// <summary>
        /// Sepetteki ürünü silme
        /// </summary>
        /// <param name="productId">Sepetten silinecek ürün id'si</param>
        /// <returns>Ok</returns>
        [HttpDelete("removeProduct/{productId}")]
        [Authorize(Roles = $"{RoleConsts.User}")]
        public async Task<IActionResult> RemoveItemFromCart(string productId)
        {
            bool isRemoved = await _cartWriteService.RemoveItemFromCart(productId, HttpContext.User.GetActiveUserId());
            return Ok(isRemoved);
        }

        /// <summary>
        /// Sepetteki ürün adetini azaltma
        /// </summary>
        /// <param name="productId">Azaltılacak ürün id'si</param>
        /// <returns>Ok</returns>
        [HttpPut("decreaseQuantity/{productId}/{quantity}")]
        [Authorize(Roles = $"{RoleConsts.User}")]
        public async Task<IActionResult> DecreaseItemQuantity(string productId, int quantity)
        {
            bool isDecreased = await _cartWriteService.DecreaseItemQuantity(productId, quantity, HttpContext.User.GetActiveUserId());
            return Ok(isDecreased);
        }

        /// <summary>
        /// Sepeti sıfırlama
        /// </summary>
        /// <returns>Ok</returns>
        [HttpPost("clear", Name = "ClearCart")]
        [Authorize(Roles = $"{RoleConsts.User}")]
        public async Task<IActionResult> ClearCart()
        {
            bool isCleared = await _cartWriteService.ClearCart(HttpContext.User.GetActiveUserId());
            return Ok(isCleared);
        }
    }
}
