using ECommerce.Business.Helpers.Products;
using ECommerce.Business.Models.Dtos.Products;
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
    /// Ürün işlemleri için gerekli endpointleri içermektedir.
    /// </summary>
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
        /// Ürünler getiriliyor
        /// </summary>
        /// <param name="productRequestFilter">Ürün filterleri</param>
        /// <returns>Ürünler getirilmektedir</returns>
        [HttpGet(Name = "GetProducts")]
        public IActionResult GetProducts([FromQuery] ProductRequestFilter productRequestFilter)
        {
            var products = _productReadService.GetProductsWhere(productRequestFilter, _ => _.IsValid);
            return Ok(products);
        }

        /// <summary>
        /// Verilen id'ye sahip ürün getirilmektedir
        /// </summary>
        /// <param name="id">Ürün id'si</param>
        /// <returns>Verilen id'deki ürün</returns>
        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _productReadService.GetProductByIdAsync(id);
            return Ok(product);
        }

        /// <summary>
        /// Ürün ekleme
        /// </summary>
        /// <param name="product">Eklenecek ürün detayları</param>
        /// <returns>Eklenen ürün</returns>
        [HttpPost(Name = "AddProduct")]
        [Authorize(Roles = $"{RoleConsts.Company}")]
        public async Task<IActionResult> AddProduct([FromBody] ProductAddDto product)
        {
            var response = await _productWriteService.AddProductAsync(product);
            return CreatedAtRoute("GetProductById", new { id = response.Id }, response);
        }

        /// <summary>
        /// Ürün foroğrafı ekleme
        /// </summary>
        /// <param name="productId">Fotoğrafı eklenecek ürünün id'si</param>
        /// <returns>Ok</returns>
        [HttpPost("img/{productId}")]
        [Authorize(Roles = $"{RoleConsts.Company}")]
        public async Task<IActionResult> UploadProductPhoto(string productId)
        {
            await IsCompaniesProduct(productId);
            bool isUploaded = await _productWriteService.UploadProductPhoto(productId);

            return Ok(isUploaded);
        }

        /// <summary>
        /// Ürün onaylama
        /// </summary>
        /// <param name="productId">ürün id'si</param>
        /// <returns>İşlemin başarılı olup olmadığı</returns>
        [HttpPost("confirm/{productId}")]
        [Authorize(Roles = $"{RoleConsts.Admin}")]
        public async Task<IActionResult> ConfirmProductToBeAdded(string productId)
        {
            bool isConfirmed = await _productWriteService.ConfirmProductToAdded(productId);
            return Ok(isConfirmed);
        }


        /// <summary>
        /// Tüm ürünleri onaylama
        /// </summary>
        /// <returns>İşlemin başarılı olup olmadığı</returns>
        [HttpPost("confirm")]
        [Authorize(Roles = $"{RoleConsts.Admin}")]
        public async Task<IActionResult> ConfirmProductToBeAdded()
        {
            var isConfirmed = await _productWriteService.ConfirmAllProductsToAdded();
            return Ok(isConfirmed);
        }


        /// <summary>
        /// Ürün foroğrafı silme
        /// </summary>
        /// <param name="productId">Fotoğrafı silinecek ürünün id'si</param>
        /// <param name="imageId">Fotoğrafı silinecek fotoğraf</param>
        /// <returns>Ok</returns>
        [HttpDelete("img/{productId}/{imageId}")]
        [Authorize(Roles = $"{RoleConsts.Company}")]
        public async Task<IActionResult> RemoveProductPhoto(string productId, string imageId)
        {
            await IsCompaniesProduct(productId);
            bool isRemoved = await _productWriteService.RemoveProductPhoto(productId, imageId);

            return Ok(isRemoved);
        }

        /// <summary>
        /// Ürün güncelleme
        /// </summary>
        /// <param name="product">Güncellenecek ürün detayları</param>
        /// <returns>Güncellenen ürün</returns>
        [HttpPut(Name = "UpdateProduct")]
        [Authorize(Roles = $"{RoleConsts.Company}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateDto product)
        {
            await IsCompaniesProduct(product.Id.ToString());
            await _productWriteService.UpdateProductAsync(product);

            return Ok(product);
        }

        /// <summary>
        /// Ürün silme
        /// </summary>
        /// <param name="id">Silinecek ürün</param>
        /// <returns>Ok</returns>
        [HttpDelete("{id}", Name = "DeleteProduct")]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Company}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            if (await _productReadService.IsCompaniesProduct(id, HttpContext.User.GetActiveUserId())
                || User.IsInRole(RoleConsts.Admin))
            {
                await _productWriteService.SafeRemoveProductAsync(id);
                return Ok();
            }

            throw new ForbiddenException();
        }

        private async Task IsCompaniesProduct(string productId)
        {
            bool isCompaniesProduct = await _productReadService.IsCompaniesProduct(productId, HttpContext.User.GetActiveUserId());
            if (!isCompaniesProduct)
                throw new ForbiddenException();
        }
    }
}
