using AutoMapper;
using ECommerce.Business.Models.Dtos.Products;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Services.WriteServices
{
    public class ProductWriteService : IProductWriteService
    {
        private readonly IWriteRepository<Product> _productWriteRepository;
        private readonly IProductReadService _productReadService;
        private readonly IProductPhotoWriteService _productPhotoWriteService;
        private readonly IMapper _mapper;

        public ProductWriteService(IUnitOfWork unitOfWork, IMapper mapper, IProductReadService productReadService,
            IProductPhotoWriteService productPhotoWriteService)
        {
            _productWriteRepository = unitOfWork.GetWriteRepository<Product>();
            _mapper = mapper;
            _productReadService = productReadService;
            _productPhotoWriteService = productPhotoWriteService;
        }

        public async Task<ProductListDto> AddProductAsync(ProductAddDto product)
        {
            var productToAdd = _mapper.Map<Product>(product);
            productToAdd.CreatedBy = Guid.Parse("48df4af4-aa79-45f2-755c-08dc51134e88");
            //productToAdd.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext.User.GetActiveUserId());
            await _productPhotoWriteService.UploadProductPhoto(productToAdd);
            bool isAdded = await _productWriteRepository.AddAsync(productToAdd);
            if (!isAdded)
                throw new InternalServerErrorException();

            var productToList = _mapper.Map<ProductListDto>(productToAdd);

            return productToList;
        }

        public async Task<bool> SafeRemoveProductAsync(string productId)
        {
            var product = await GetSingleProductIncludeProductPhotos(productId, true);
            bool isDeleted = await _productWriteRepository.SafeRemoveAsync(product);
            if (isDeleted && product.ProductPhotos != null)
                await _productPhotoWriteService.RemoveProductPhotos(product.ProductPhotos.ToList());

            return isDeleted;
        }

        public async Task SafeRemoveProductRangeAsync(List<string> productIds)
        {
            var products = _productReadService
                .Products
                .GetWhere(_ => productIds.Contains(_.Id.ToString()) && _.IsValid, false, [_ => _.ProductPhotos]);
            foreach (var product in products)
            {
                bool isRemoved = await _productWriteRepository.SafeRemoveAsync(product);
                if (isRemoved && product.ProductPhotos != null)
                    await _productPhotoWriteService.RemoveProductPhotos(product.ProductPhotos.ToList());
            }
        }

        public async Task<bool> UpdateProductAsync(ProductUpdateDto product)
        {
            var productToUpdate = await GetSingleProduct(product.Id.ToString());
            _mapper.Map(product, productToUpdate);

            return await _productWriteRepository.UpdateAsync(productToUpdate);
        }

        public async Task<bool> DecreaseProductQuantity(string productId, int quantity)
        {
            var product = await _productReadService.Products.GetByIdAsync(productId);
            if (product is null)
                return false;
            if (product.Stock < quantity)
                throw new BadRequestException($"Stokta {product.ProductName}'den {product.Stock} adet bulunmaktadır!");

            product.Stock -= quantity;

            return await _productWriteRepository.UpdateAsync(product);
        }

        public async Task<bool> UploadProductPhoto(string productId)
        {
            var product = await GetSingleProductIncludeProductPhotos(productId, true);
            await _productPhotoWriteService.ThrowErrorIfImageLimitIsExceed(product);

            return await _productPhotoWriteService.UploadProductPhoto(product);
        }

        public async Task<bool> RemoveProductPhoto(string productId, string imageId)
        {
            var product = await _productReadService.Products.GetSingleAsync(_ => _.Id == Guid.Parse(productId), false, [_ => _.ProductPhotos]);
            if (product is null)
                throw new NotFoundException("Ürün bulunamadı!");

            return await _productPhotoWriteService.RemoveProductPhoto(product, imageId);
        }

        private async Task<Product> GetSingleProduct(string productId)
        {
            var product = await _productReadService.Products.GetByIdAsync(productId);
            return product is null || !product.IsValid
            ? throw new NotFoundException("Ürün bulunamadı!")
            : product;
        }

        private async Task<Product> GetSingleProductIncludeProductPhotos(string productId, bool isValid, bool allowTracking = false)
        {
            var product = await _productReadService.Products.GetSingleAsync(_ => _.Id == Guid.Parse(productId) && _.IsValid == isValid, allowTracking, [_ => _.ProductPhotos]);
            if (product is null)
                throw new NotFoundException("Ürün bulunamadı!");

            return product;
        }
    }
}
