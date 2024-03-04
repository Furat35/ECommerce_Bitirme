using ECommerce.Business.ActionFilters;
using ECommerce.Business.Models.Dtos.CartItems;
using ECommerce.Business.Services.Carts.Abstract;
using ECommerce.Business.Validations.FluentValidations.CartItems;
using ECommerce.Core.Consts;
using ECommerce.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
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
        /// Sepet getiriliyor.
        /// </summary>
        /// <returns>Sepet getirilmektedir.</returns>
        [HttpGet(Name = "GetCart")]
        [Authorize(Roles = $"{RoleConsts.User}")]
        public async Task<IActionResult> GetCart()
        {
            var carts = await _cartReadService.GetCartAsync(HttpContext.User.GetActiveUserId());
            return Ok(carts);
        }

        /// <summary>
        /// Sepet ekleme
        /// </summary>
        /// <param name="cartItem">Sepete eklenecek ürün detayları</param>
        /// <returns>Eklenen ürün</returns>
        [HttpPut(Name = "UpdateCart")]
        [Authorize(Roles = $"{RoleConsts.User}")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<CartItemAddDtoValidator, CartItemAddDto>), Arguments = ["cartItem"])]
        public async Task<IActionResult> UpdateCart([FromBody] CartItemAddDto cartItem)
        {
            var response = await _cartWriteService.AddItemToCart(cartItem, HttpContext.User.GetActiveUserId());
            return Ok(response);
        }

        /// <summary>
        /// Sepeti sıfırlama
        /// </summary>
        /// <returns>Ok</returns>
        [HttpPost("clear", Name = "ClearCart")]
        [Authorize(Roles = $"{RoleConsts.User}")]
        public async Task<IActionResult> ClearCart()
        {
            var response = await _cartWriteService.ClearCart(HttpContext.User.GetActiveUserId());
            return Ok(response);
        }
    }
}
