using ECommerce.Business.Services.Contracts.IReadServices;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    /// <summary>
    /// Adres ile ilgili işlemlerin yapıldığı endpointleri içermektedir.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressReadService _addressReadService;

        public AddressController(IAddressReadService addressReadService)
        {
            _addressReadService = addressReadService;
        }

        /// <summary>
        /// Ülkeler getiriliyor
        /// </summary>
        /// <returns>Ülkeler getirilmektedir</returns>
        [HttpGet("countries")]
        public IActionResult GetCountries()
        {
            var country = _addressReadService.GetCountries();
            return Ok(country);
        }

        /// <summary>
        /// Verilen id'deki ülke getiriliyor
        /// </summary>
        /// <param name="id">Ülke id'si</param>
        /// <returns>Verilen id'deki ülke</returns>     
        [HttpGet("countries/{id}")]
        public IActionResult GetCountryById(string id)
        {
            var country = _addressReadService.GetCountryById(id);
            return Ok(country);
        }

        /// <summary>
        /// Verilen ülke id'sine sahip İller getiriliyor
        /// </summary>
        /// <param name="id">Ülke id'si</param>
        /// <returns>Verilen ülke id'sine sahip şehirler getirilmektedir</returns>     
        [HttpGet("cities/{id}")]
        public IActionResult GetCitiesByCountryId(string id)
        {
            var country = _addressReadService.GetCitiesByCountryId(id);
            return Ok(country);
        }

        /// <summary>
        /// Verilen şehir id'sine sahip İlçeler getiriliyor
        /// </summary>
        /// <param name="id">Şehir id'si</param>
        /// <returns>Verilen şehir id'sine sahip İlçeler getirilmektedir</returns>     
        [HttpGet("districts/{id}")]
        public IActionResult GetDistrictsByCityId(string id)
        {
            var country = _addressReadService.GetDistrictsByCityId(id);
            return Ok(country);
        }
    }
}
