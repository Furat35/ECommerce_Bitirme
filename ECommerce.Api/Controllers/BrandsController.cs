using ECommerce.Business.ActionFilters;
using ECommerce.Business.Helpers.Brands;
using ECommerce.Business.Models.Dtos.Brands;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Business.Validations.FluentValidations.Brands;
using ECommerce.Core.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    /// <summary>
    /// Bir ürün markası için gerekli crud işlemlerinin yapıldığı endpointleri içermektedir.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandReadService _brandReadService;
        private readonly IBrandWriteService _brandWriteService;

        public BrandsController(IBrandReadService brandReadService, IBrandWriteService brandWriteService)
        {
            _brandReadService = brandReadService;
            _brandWriteService = brandWriteService;
        }

        /// <summary>
        /// Markalar getiriliyor
        /// </summary>
        /// <param name="brandRequestFilter">Marka filterleri</param>
        /// <returns>Markalar getirilmektedir</returns>
        [HttpGet(Name = "GetBrands")]
        public IActionResult GetBrands([FromQuery] BrandRequestFilter brandRequestFilter)
        {
            var brands = _brandReadService.GetBrandsWhere(brandRequestFilter, _ => _.IsValid);
            return Ok(brands);
        }

        /// <summary>
        /// Verilen id'ye sahip marka getirilmektedir
        /// </summary>
        /// <param name="id">Marka id'si</param>
        /// <returns>Verilen id'deki marka</returns>
        [HttpGet("{id}", Name = "GetBrandById")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = ["id"])]
        public async Task<IActionResult> GetBrandById(string id)
        {
            var brand = await _brandReadService.GetBrandByIdAsync(id);
            return Ok(brand);
        }

        /// <summary>
        /// Marka ekleme
        /// </summary>
        /// <param name="brand">Eklenecek marka detayları</param>
        /// <returns>Eklenen marka</returns>
        [HttpPost(Name = "AddBrand")]
        [Authorize(Roles = $"{RoleConsts.Admin}")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<BrandAddDtoValidator, BrandAddDto>), Arguments = ["brand"])]
        public async Task<IActionResult> AddBrand([FromBody] BrandAddDto brand)
        {
            var response = await _brandWriteService.AddBrandAsync(brand);
            return CreatedAtRoute("GetBrandById", new { id = response.Id }, response);
        }

        /// <summary>
        /// Marka güncelleme
        /// </summary>
        /// <param name="brand">Güncellenecek marka detayları</param>
        /// <returns>Güncellenen marka</returns>
        [HttpPut(Name = "UpdateBrand")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<BrandUpdateDtoValidator, BrandUpdateDto>), Arguments = ["brand"])]
        [Authorize(Roles = $"{RoleConsts.Admin}")]
        public async Task<IActionResult> UpdateBrand([FromBody] BrandUpdateDto brand)
        {
            bool isUpdated = await _brandWriteService.UpdateBrandAsync(brand);
            return Ok(isUpdated);
        }

        /// <summary>
        /// Marka silme
        /// </summary>
        /// <param name="id">Silinecek marka id'si</param>
        /// <returns>Ok</returns>
        [HttpDelete("{id}", Name = "DeleteBrand")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = ["id"])]
        [Authorize(Roles = $"{RoleConsts.Admin}")]
        public async Task<IActionResult> DeleteBrand(string id)
        {
            bool isRemoved = await _brandWriteService.SafeRemoveBrandAsync(id);
            return Ok(isRemoved);
        }
    }
}
