using ECommerce.Business.ActionFilters;
using ECommerce.Business.Helpers.Categories;
using ECommerce.Business.Models.Dtos.Categories;
using ECommerce.Business.Services.Categories.Abstract;
using ECommerce.Business.Validations.FluentValidations.Categories;
using ECommerce.Core.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryReadService _categoryReadService;
        private readonly ICategoryWriteService _categoryWriteService;

        public CategoriesController(ICategoryReadService categoryReadService, ICategoryWriteService categoryWriteService)
        {
            _categoryReadService = categoryReadService;
            _categoryWriteService = categoryWriteService;
        }

        /// <summary>
        /// Kategoriler getiriliyor.
        /// </summary>
        /// <param name="categoryRequestFilter">Kategori filtreleri</param>
        /// <returns>Kategoriler getirilmektedir.</returns>
        [HttpGet(Name = "GetCategories")]
        public IActionResult GetCategories([FromQuery] CategoryRequestFilter categoryRequestFilter)
        {
            var categorys = _categoryReadService.GetCategoriesWhere(categoryRequestFilter, _ => _.IsValid);
            return Ok(categorys);
        }

        /// <summary>
        /// Verilen id'ye sahip kategori getirilmektedir.
        /// </summary>
        /// <param name="id">Kategori id'si</param>
        /// <returns>Verilen id'deki kategori</returns>
        [HttpGet("{id}", Name = "GetCategoryById")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = ["id"])]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var category = await _categoryReadService.GetCategoryByIdAsync(id);
            return Ok(category);
        }

        /// <summary>
        /// Kategori ekleme
        /// </summary>
        /// <param name="category">Kategori detayları</param>
        /// <returns>Eklenen kategori</returns>
        [HttpPost(Name = "AddCategory")]
        [Authorize(Roles = $"{RoleConsts.Admin}")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<CategoryAddDtoValidator, CategoryAddDto>), Arguments = ["category"])]
        public async Task<IActionResult> AddCategory([FromBody] CategoryAddDto category)
        {
            var response = await _categoryWriteService.AddCategoryAsync(category);
            return CreatedAtRoute("GetCategoryById", new { id = response.Id }, response);
        }


        /// <summary>
        /// Kategori güncelleme
        /// </summary>
        /// <param name="category">Güncellenecek kategori detayları</param>
        /// <returns>Güncellenen kategori</returns>
        [HttpPut(Name = "UpdateCategory")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<CategoryUpdateDtoValidator, CategoryUpdateDto>), Arguments = ["category"])]
        [Authorize(Roles = $"{RoleConsts.Admin}")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryUpdateDto category)
        {
            await _categoryWriteService.UpdateCategoryAsync(category);
            return Ok(category);
        }

        /// <summary>
        /// Kategori silme
        /// </summary>
        /// <param name="id">Silinecek kategori</param>
        /// <returns>Ok</returns>
        [HttpDelete("{id}", Name = "DeleteCategory")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = ["id"])]
        [Authorize(Roles = $"{RoleConsts.Admin}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var result = await _categoryWriteService.SafeRemoveCategoryAsync(id);
            return Ok(result);
        }
    }
}
