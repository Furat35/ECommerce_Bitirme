using ECommerce.Business.Services.Addresses.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
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

        [HttpGet("countries")]
        public IActionResult GetCountries()
        {
            var country = _addressReadService.GetCountries();
            return Ok(country);
        }

        [HttpGet("countries/{id}")]
        public IActionResult GetCountryById(string id)
        {
            var country = _addressReadService.GetCountryById(id);
            return Ok(country);
        }
        
        [HttpGet("cities/{id}")]
        public IActionResult GetCitiesByCountryId(string id)
        {
            var country = _addressReadService.GetCitiesByCountryId(id);
            return Ok(country);
        }

        [HttpGet("districts/{id}")]
        public IActionResult GetDistrictsByCityId(string id)
        {
            var country = _addressReadService.GetDistrictsByCityId(id);
            return Ok(country);
        }
    }
}
