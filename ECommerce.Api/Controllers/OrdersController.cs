using ECommerce.Business.ActionFilters;
using ECommerce.Business.Models.Dtos.Categories;
using ECommerce.Business.Models.Dtos.Orders;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Business.Validations.FluentValidations.Categories;
using ECommerce.Core.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderWriteService _orderWriteService;

        public OrdersController(IOrderWriteService orderWriteService)
        {
            _orderWriteService = orderWriteService;
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.User}")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<CategoryAddDtoValidator, CategoryAddDto>), Arguments = ["category"])]
        public async Task<IActionResult> CheckoutOrder([FromBody] OrderCheckoutDto order)
        {
            await _orderWriteService.CheckoutOrder(order);
            return Ok();
        }

        //[HttpGet]
        //[Authorize(Roles = $"{RoleConsts.Company}")]
        //[TypeFilter(typeof(FluentValidationFilterAttribute<CategoryAddDtoValidator, CategoryAddDto>), Arguments = ["category"])]
        //public async Task<IActionResult> GetCompletedCompanyOrders([FromBody] OrderCheckoutDto order)
        //{
        //    await _orderWriteService.CheckoutOrder(order);
        //    return Ok();
        //}
    }
}
