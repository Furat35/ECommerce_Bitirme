using ECommerce.Business.Helpers.FilterServices.OrderItems;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Core.Consts;
using ECommerce.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    /// <summary>
    /// Siparişteki ürünler ile ilgili gerekli endpointleri içermektedir
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemReadService _orderItemReadService;
        private readonly IOrderItemWriteService _orderItemWriteService;

        public OrderItemsController(IOrderItemReadService orderItemReadService, IOrderItemWriteService orderItemWriteService)
        {
            _orderItemReadService = orderItemReadService;
            _orderItemWriteService = orderItemWriteService;
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Company}")]
        public async Task<IActionResult> GetOrderItemsWithGivenStatus([FromQuery] OrderItemRequestFilter filters)
        {
            string userId = HttpContext.User.GetActiveUserId();
            var orderItems = await _orderItemReadService.GetOrderItemsWithGivenStatus(filters, userId);

            return Ok(orderItems);
        }

        [HttpPost("{orderItemId}/{orderItemStatus}")]
        [Authorize(Roles = $"{RoleConsts.Company}")]
        public async Task<IActionResult> SetOrderItemStatus(string orderItemId, Entity.Enums.OrderItemStatus orderItemStatus)
        {
            string userId = HttpContext.User.GetActiveUserId();
            bool hasAccess = await _orderItemReadService.CheckIfOrderItemBelongsToGivenUser(orderItemId, userId);
            if (!hasAccess)
                throw new FormatException();

            bool statusIsChanged = await _orderItemWriteService.SetOrderItemStatus(orderItemId, orderItemStatus);

            return Ok(statusIsChanged);
        }
    }
}
