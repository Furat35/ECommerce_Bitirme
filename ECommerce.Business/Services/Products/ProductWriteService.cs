using AutoMapper;
using ECommerce.Business.Models.Dtos.Products;
using ECommerce.Business.Services.ImageService;
using ECommerce.Business.Services.Products.Abstract;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.Core.Extensions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Business.Services.Products
{
    public class ProductWriteService : IProductWriteService
    {
        private readonly IWriteRepository<Product> _productWriteRepository;
        private readonly IProductReadService _productReadService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public ProductWriteService(IUnitOfWork unitOfWork, IMapper mapper, IProductReadService productReadService,
            IHttpContextAccessor httpContextAccessor, IFileService fileService)
        {
            _productWriteRepository = unitOfWork.GetWriteRepository<Product>();
            _mapper = mapper;
            _productReadService = productReadService;
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
        }

        public async Task<ProductListDto> AddProductAsync(ProductAddDto product)
        {
            var productToAdd = _mapper.Map<Product>(product);
            productToAdd.UserId = Guid.Parse(_httpContextAccessor.HttpContext.User.GetActiveUserId());
            await _productWriteRepository.AddAsync(productToAdd);
            //_fileService.UploadFile("productimages", _httpContextAccessor.HttpContext.Request.Form)
            await SaveChangesAsync();
            var productToList = _mapper.Map<ProductListDto>(productToAdd);

            return productToList;
        }

        public async Task<bool> SafeRemoveProductAsync(string productId)
        {
            var product = await GetSingleProduct(productId);
            product.DeletedDate = DateTime.UtcNow;
            _productWriteRepository.SafeRemove(product);

            return await SaveChangesAsync();
        }

        public async Task<bool> SafeRemoveProductRangeAsync(List<string> productIds)
        {
            var products = _productReadService
                .Products
                .GetWhere(_ => productIds.Contains(_.Id.ToString()) && _.IsValid);
            foreach (var product in products)
            {
                product.DeletedDate = DateTime.UtcNow;
                _productWriteRepository.SafeRemove(product);
            }

            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateProductAsync(ProductUpdateDto product)
        {
            var productToUpdate = await GetSingleProduct(product.Id.ToString());
            _mapper.Map(product, productToUpdate);
            _productWriteRepository.Update(productToUpdate);

            return await SaveChangesAsync();
        }

        public async Task<bool> IsProductCreatedByUserAsync(string productId, string userId)
        {
            var product = await GetSingleProduct(productId);
            return product.UserId.ToString().Equals(userId, StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task<bool> SaveChangesAsync()
            => await _productWriteRepository.SaveAsync() != 0;

        private async Task<Product> GetSingleProduct(string productId)
        {
            var product = await _productReadService.Products.GetByIdAsync(productId);
            return product is null || !product.IsValid
            ? throw new NotFoundException("Ürün bulunamadı!")
            : product;
        }
    }
}
