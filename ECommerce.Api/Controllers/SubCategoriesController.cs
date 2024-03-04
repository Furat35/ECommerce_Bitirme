using ECommerce.Business.ActionFilters;
using ECommerce.Business.Helpers.SubCategories;
using ECommerce.Business.Models.Dtos.SubCategories;
using ECommerce.Business.Services.SubCategories.Abstract;
using ECommerce.Business.Validations.FluentValidations.SubCategories;
using ECommerce.Core.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SubCategoriesController : ControllerBase
    {
        private readonly ISubCategoryReadService _subCategoryReadService;
        private readonly ISubCategoryWriteService _subCategoryWriteService;

        public SubCategoriesController(ISubCategoryReadService categoryReadService, ISubCategoryWriteService categoryWriteService)
        {
            _subCategoryReadService = categoryReadService;
            _subCategoryWriteService = categoryWriteService;
        }

        /// <summary>
        /// Alt Kategoriler getiriliyor.
        /// </summary>
        /// <param name="subCategoryRequestFilter">Alt Kategori filtreleri</param>
        /// <returns>Alt Kategoriler getirilmektedir.</returns>
        [HttpGet(Name = "GetSubCategories")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = ["categoryId"])]
        public IActionResult GetSubCategories([FromQuery] SubCategoryRequestFilter subCategoryRequestFilter, [FromQuery] string categoryId)
        {
            var subCategories = _subCategoryReadService.GetSubCategoriesWhere(subCategoryRequestFilter, _ => _.IsValid && _.CategoryId.ToString() == categoryId);
            return Ok(subCategories);
        }

        /// <summary>
        /// Verilen id'ye sahip alt kategori getirilmektedir.
        /// </summary>
        /// <param name="id">Alt kategori id'si</param>
        /// <returns>Verilen id'deki kategori</returns>
        [HttpGet("{id}", Name = "GetSubCategoryById")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = ["id"])]
        public async Task<IActionResult> GetSubCategoryById(string id)
        {
            var subCategory = await _subCategoryReadService.GetSubCategoryByIdAsync(id);
            return Ok(subCategory);
        }

        /// <summary>
        /// Alt Kategori ekleme
        /// </summary>
        /// <param name="subCategory">Alt Kategori detayları</param>
        /// <returns>Eklenen alt kategori</returns>
        [HttpPost(Name = "AddSubCategory")]
        [Authorize(Roles = $"{RoleConsts.Admin}")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<SubCategoryAddDtoValidator, SubCategoryAddDto>), Arguments = ["subCategory"])]
        public async Task<IActionResult> AddSubCategory([FromBody] SubCategoryAddDto subCategory)
        {
            var response = await _subCategoryWriteService.AddSubCategoryAsync(subCategory);
            return CreatedAtRoute("GetCategoryById", new { id = response.Id }, response);
        }

        /// <summary>
        /// Alt Kategori güncelleme
        /// </summary>
        /// <param name="subCategory">Güncellenecek alt kategori detayları</param>
        /// <returns>Güncellenen alt kategori</returns>
        [HttpPut(Name = "UpdateSubCategory")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<SubCategoryUpdateDtoValidator, SubCategoryUpdateDto>), Arguments = ["subCategory"])]
        [Authorize(Roles = $"{RoleConsts.Admin}")]
        public async Task<IActionResult> UpdateSubCategory([FromBody] SubCategoryUpdateDto subCategory)
        {
            await _subCategoryWriteService.UpdateSubCategoryAsync(subCategory);
            return Ok(subCategory);
        }

        /// <summary>
        /// Alt Kategori silme
        /// </summary>
        /// <param name="id">Silinecek alt kategori</param>
        /// <returns>Ok</returns>
        [HttpDelete("{id}", Name = "DeleteSubCategory")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = ["id"])]
        [Authorize(Roles = $"{RoleConsts.Admin}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var result = await _subCategoryWriteService.SafeRemoveSubCategoryAsync(id);
            return Ok(result);
        }
    }
}
