using ECommerce.Business.Helpers.FilterServices.Orders;
using ECommerce.Business.Models.Dtos.Orders;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Core.Consts;
using ECommerce.Core.Exceptions;
using ECommerce.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    /// <summary>
    /// Siparişler ile ilgili gerekli endpointleri içermektedir.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderWriteService _orderWriteService;
        private readonly IOrderReadService _orderReadService;

        public OrdersController(IOrderWriteService orderWriteService, IOrderReadService orderReadService)
        {
            _orderWriteService = orderWriteService;
            _orderReadService = orderReadService;
        }

        /// <summary>
        /// Verilen id'deki sipariş getirilmektedir
        /// </summary>
        /// <param name="orderId">Sipariş id'si</param>
        /// <returns>Verilen id'deki sipariş</returns>
        [HttpGet("{orderId}")]
        [Authorize(Roles = $"{RoleConsts.User}")]
        public async Task<IActionResult> GetOrderById(string orderId)
        {
            string userId = HttpContext.User.GetActiveUserId();
            bool checkIfOrderBelongsToActiveUser = await _orderReadService.CheckIfOrderIsCreatedByGivenUser(orderId, userId);
            if (!checkIfOrderBelongsToActiveUser)
                throw new ForbiddenException();

            var order = await _orderReadService.GetOrderById(orderId);
            return Ok(order);
        }

        /// <summary>
        /// Sipariş oluşturma
        /// </summary>
        /// <param name="order">Sipariş bilgileri</param>
        /// <returns>Ok</returns>
        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.User}")]
        public async Task<IActionResult> CheckoutOrder([FromBody] OrderCheckoutDto order)
        {
            await _orderWriteService.CheckoutOrder(order);
            return Ok();
        }

        /// <summary>
        /// Verilen filtreye göre siparişler getirilmektedir
        /// </summary>
        /// <param name="filters">Sipariş filtreleri</param>
        /// <returns>Verilen filtredeki siparişler</returns>
        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.User}")]
        public async Task<IActionResult> GetOrdersWithGivenStatus([FromQuery] OrderRequestFilter filters)
        {
            string userId = HttpContext.User.GetActiveUserId();
            var notCompletedOrders = await _orderReadService.GetOrdersWithGivenStatus(filters, userId);

            return Ok(notCompletedOrders);
        }

        /// <summary>
        /// Sipariş durumunu değiştirme
        /// </summary>
        /// <param name="orderId">Sipariş id'si</param>
        /// <param name="orderStatus">Sipairşin yeni durumu</param>
        /// <returns>İşlemin başarılı olup olmadığı</returns>
        [HttpPost("{orderId}/{orderStatus}")]
        [Authorize(Roles = $"{RoleConsts.User}")]
        public async Task<IActionResult> SetOrderStatus(string orderId, Entity.Enums.OrderStatus orderStatus)
        {
            string userId = HttpContext.User.GetActiveUserId();
            bool checkIfOrderBelongsToActiveUser = await _orderReadService.CheckIfOrderIsCreatedByGivenUser(orderId, userId);
            if (!checkIfOrderBelongsToActiveUser)
                throw new ForbiddenException();
            bool isCompleted = await _orderWriteService.ChangeOrderStatus(orderStatus, orderId);

            return Ok(isCompleted);
        }
    }
}
