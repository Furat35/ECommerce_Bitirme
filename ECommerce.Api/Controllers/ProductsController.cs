using ECommerce.Business.ActionFilters;
using ECommerce.Business.Helpers.Products;
using ECommerce.Business.Models.Dtos.Products;
using ECommerce.Business.Services.Products.Abstract;
using ECommerce.Business.Validations.FluentValidations.Products;
using ECommerce.Core.Consts;
using ECommerce.Core.Exceptions;
using ECommerce.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductReadService _productReadService;
        private readonly IProductWriteService _productWriteService;

        public ProductsController(IProductReadService productReadService, IProductWriteService productWriteService)
        {
            _productReadService = productReadService;
            _productWriteService = productWriteService;
        }

        /// <summary>
        /// Ürünler getiriliyor.
        /// </summary>
        /// <param name="productRequestFilter">Ürün filterleri</param>
        /// <returns>Ürünler getirilmektedir.</returns>
        [HttpGet(Name = "GetProducts")]
        public IActionResult GetProducts([FromQuery] ProductRequestFilter productRequestFilter)
        {
            var products = _productReadService.GetProductsWhere(productRequestFilter, _ => _.IsValid);
            return Ok(products);
        }

        /// <summary>
        /// Verilen id'ye sahip ürün getirilmektedir.
        /// </summary>
        /// <param name="id">Ürün id'si</param>
        /// <returns>Verilen id'deki ürün</returns>
        [HttpGet("{id}", Name = "GetProductById")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = ["id"])]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _productReadService.GetProductIdAsync(id);
            return Ok(product);
        }

        /// <summary>
        /// Ürün ekleme
        /// </summary>
        /// <param name="product">Ürün detayları</param>
        /// <returns>Eklenen ürün</returns>
        [HttpPost(Name = "AddProduct")]
        [Authorize(Roles = $"{RoleConsts.Company}")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<ProductAddDtoValidator, ProductAddDto>), Arguments = ["product"])]
        public async Task<IActionResult> AddProduct([FromBody] ProductAddDto product)
        {
            var response = await _productWriteService.AddProductAsync(product);
            return CreatedAtRoute("GetProductById", new { id = response.Id }, response);
        }

        /// <summary>
        /// Ürün güncelleme
        /// </summary>
        /// <param name="product">Güncellenecek ürün detayları</param>
        /// <returns>Güncellenen ürün</returns>
        [HttpPut(Name = "UpdateProduct")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<ProductUpdateDtoValidator, ProductUpdateDto>), Arguments = ["product"])]
        [Authorize(Roles = $"{RoleConsts.Company}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateDto product)
        {
            if (!await _productWriteService.IsProductCreatedByUserAsync(product.Id.ToString(), HttpContext.User.GetActiveUserId()))
                throw new ForbiddenException();

            await _productWriteService.UpdateProductAsync(product);
            return Ok(product);
        }

        /// <summary>
        /// Ürün silme
        /// </summary>
        /// <param name="id">Silinecek ürün</param>
        /// <returns>Ok</returns>
        [HttpDelete("{id}", Name = "DeleteProduct")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = ["id"])]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Company}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            if (await _productWriteService.IsProductCreatedByUserAsync(id, HttpContext.User.GetActiveUserId())
                || User.IsInRole(RoleConsts.Admin))
            {
                await _productWriteService.SafeRemoveProductAsync(id);
                return Ok();
            }

            throw new ForbiddenException();
        }
    }
}
