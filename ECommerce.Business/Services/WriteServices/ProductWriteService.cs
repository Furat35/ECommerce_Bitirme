using AutoMapper;
using ECommerce.Business.Extensions;
using ECommerce.Business.Models.Dtos.Products;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.Core.Extensions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Business.Services.WriteServices
{
    public class ProductWriteService : IProductWriteService
    {
        private readonly IWriteRepository<Product> _productWriteRepository;
        private readonly IProductReadService _productReadService;
        private readonly IProductPhotoWriteService _productPhotoWriteService;
        private readonly ISubCategoryReadService _subCategoryReadService;
        private readonly IBrandReadService _brandReadService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IValidator<ProductAddDto> _productAddDtoValidator;
        private readonly IValidator<ProductUpdateDto> _productUpdateDtoValidator;

        public ProductWriteService(IUnitOfWork unitOfWork, IMapper mapper, IProductReadService productReadService,
            IProductPhotoWriteService productPhotoWriteService, ISubCategoryReadService subCategoryReadService, IBrandReadService brandReadService,
            IHttpContextAccessor httpContextAccessor, IValidator<ProductAddDto> productAddDtoValidator, IValidator<ProductUpdateDto> productUpdateDtoValidator)
        {
            _productWriteRepository = unitOfWork.GetWriteRepository<Product>();
            _mapper = mapper;
            _productReadService = productReadService;
            _productPhotoWriteService = productPhotoWriteService;
            _subCategoryReadService = subCategoryReadService;
            _brandReadService = brandReadService;
            _httpContextAccessor = httpContextAccessor;
            _productAddDtoValidator = productAddDtoValidator;
            _productUpdateDtoValidator = productUpdateDtoValidator;
        }

        public async Task<ProductListDto> AddProductAsync(ProductAddDto product)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(product, _productAddDtoValidator);
            // Check if brand and subcategory exists
            await _brandReadService.GetBrandByIdAsync(product.BrandId.ToString());
            await _subCategoryReadService.GetSubCategoryByIdAsync(product.SubCategoryId.ToString());

            var productToAdd = _mapper.Map<Product>(product);
            productToAdd.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext.User.GetActiveUserId());
            try
            {
                await _productPhotoWriteService.UploadProductPhoto(productToAdd);
            }
            catch (Exception)
            {
                //log
            }
            bool isAdded = await _productWriteRepository.AddAsync(productToAdd);
            if (!isAdded)
                throw new InternalServerErrorException();

            var productToList = _mapper.Map<ProductListDto>(productToAdd);

            return productToList;
        }

        public async Task<bool> SafeRemoveProductAsync(string productId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(productId);
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
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(product, _productUpdateDtoValidator);
            var productToUpdate = await GetSingleProduct(product.Id.ToString());
            _mapper.Map(product, productToUpdate);

            return await _productWriteRepository.UpdateAsync(productToUpdate);
        }

        public async Task<bool> DecreaseProductQuantity(string productId, int quantity)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(productId);
            var product = await _productReadService.Products.GetByIdAsync(productId);
            if (product is null)
                throw new NotFoundException("Ürün bulunamadı!");
            if (product.Stock < quantity)
                throw new BadRequestException($"Stokta {product.ProductName}'den {product.Stock} adet bulunmaktadır! Geçerli adet bilgisi giriniz");

            product.Stock -= quantity;

            return await _productWriteRepository.UpdateAsync(product);
        }

        public async Task<bool> UploadProductPhoto(string productId)
        {
            var product = await GetSingleProductIncludeProductPhotos(productId, true);
            await _productPhotoWriteService.ThrowErrorIfImageLimitIsExceed(product);
            try
            {
                return await _productPhotoWriteService.UploadProductPhoto(product);
            }
            catch (Exception ex)
            {
                //log
                return false;
            }
        }

        public async Task<bool> RemoveProductPhoto(string productId, string imageId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(productId, imageId);
            var product = await _productReadService.Products.GetSingleAsync(_ => _.Id == Guid.Parse(productId), false, [_ => _.ProductPhotos]);
            if (product is null)
                throw new NotFoundException("Ürün bulunamadı!");

            return await _productPhotoWriteService.RemoveProductPhoto(product, imageId);
        }

        public async Task<bool> ConfirmProductToAdded(string productId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(productId);
            var product = await _productReadService.Products.GetByIdAsync(productId);
            if (product is null)
                throw new NotFoundException("Ürün bulunamadı!");
            if (product.IsValid)
                return false;
            product.IsValid = true;

            return await _productWriteRepository.UpdateAsync(product);
        }

        public async Task<bool> ConfirmAllProductsToAdded()
        {
            var products = _productReadService.Products.GetWhere(_ => !_.IsValid);
            foreach (var product in products)
                product.IsValid = true;

            return await _productWriteRepository.UpdateRangeAsync(products.ToList());
        }


        private async Task<Product> GetSingleProduct(string productId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(productId);
            var product = await _productReadService.Products.GetByIdAsync(productId);

            return product is null || !product.IsValid
            ? throw new NotFoundException("Ürün bulunamadı!")
            : product;
        }

        private async Task<Product> GetSingleProductIncludeProductPhotos(string productId, bool isValid, bool allowTracking = false)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(productId);
            var product = await _productReadService.Products.GetSingleAsync(_ => _.Id == Guid.Parse(productId) && _.IsValid == isValid, allowTracking, [_ => _.ProductPhotos]);
            if (product is null)
                throw new NotFoundException("Ürün bulunamadı!");

            return product;
        }
    }
}
