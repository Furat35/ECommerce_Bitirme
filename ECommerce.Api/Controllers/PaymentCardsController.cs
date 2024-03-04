using ECommerce.Business.ActionFilters;
using ECommerce.Business.Models.Dtos.PaymentCards;
using ECommerce.Business.Services.PaymentCards.Abstract;
using ECommerce.Business.Validations.FluentValidations.PaymentCards;
using ECommerce.Core.Consts;
using ECommerce.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentCardsController : ControllerBase
    {
        private readonly IPaymentCardReadService _paymentCardReadService;
        private readonly IPaymentCardWriteService _paymentCardWriteService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentCardsController(IPaymentCardReadService paymentCardReadService, IPaymentCardWriteService paymentCardWriteService, IHttpContextAccessor httpContextAccessor)
        {
            _paymentCardReadService = paymentCardReadService;
            _paymentCardWriteService = paymentCardWriteService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Verilen id'ye sahip kart getirilmektedir.
        /// </summary>
        /// <returns>Verilen id'deki kart</returns>
        [HttpGet(Name = "GetPaymentCardById")]
        [Authorize(Roles = $"{RoleConsts.User},{RoleConsts.Company}")]
        public async Task<IActionResult> GetPaymentCard()
        {
            var paymentCard = await _paymentCardReadService.GetPaymentCardByUserIdAsync(_httpContextAccessor.HttpContext.User.GetActiveUserId());
            return Ok(paymentCard);
        }

        /// <summary>
        /// Kart ekleme
        /// </summary>
        /// <param name="paymentCard">Eklenecek kart detayları</param>
        /// <returns>Eklenen kart</returns>
        [HttpPost(Name = "AddPaymentCard")]
        [Authorize(Roles = $"{RoleConsts.User},{RoleConsts.Company}")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<PaymentCardAddDtoValidator, PaymentCardAddDto>), Arguments = ["paymentCard"])]
        public async Task<IActionResult> AddPaymentCard([FromBody] PaymentCardAddDto paymentCard)
        {
            var response = await _paymentCardWriteService.AddPaymentCardAsync(paymentCard);
            return CreatedAtRoute("GetPaymentCardById", new { id = response.Id }, response);
        }

        /// <summary>
        /// Kart güncelleme
        /// </summary>
        /// <param name="paymentCard">Güncellenecek kart detayları</param>
        /// <returns>Güncellenen kart</returns>
        [HttpPut(Name = "UpdatePaymentCard")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<PaymentCardUpdateDtoValidator, PaymentCardUpdateDto>), Arguments = ["paymentCard"])]
        [Authorize(Roles = $"{RoleConsts.User},{RoleConsts.Company}")]
        public async Task<IActionResult> UpdatePaymentCard([FromBody] PaymentCardUpdateDto paymentCard)
        {
            await _paymentCardWriteService.UpdatePaymentCardAsync(paymentCard);
            return Ok(paymentCard);
        }
    }
}
