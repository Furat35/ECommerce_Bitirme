using AutoMapper;
using ECommerce.Business.Helpers.FilterServices;
using ECommerce.Business.Helpers.HeaderServices;
using ECommerce.Business.Helpers.Products;
using ECommerce.Business.Models.Dtos.ProductPhotos;
using ECommerce.Business.Models.Dtos.Products;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.ImageService;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace ECommerce.Business.Services.ReadServices
{
    public class ProductReadService : IProductReadService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IFileService _fileService;

        public ProductReadService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor, IFileService fileService)
        {
            Products = unitOfWork.GetReadRepository<Product>();
            _mapper = mapper;
            _httpContext = contextAccessor;
            _fileService = fileService;
        }

        public IReadRepository<Product> Products { get; }

        public async Task<ProductListDto> GetProductIdAsync(string productId)
        {
            var product = await Products.GetByIdAsync(productId, includeProperties: [_ => _.SubCategory, _ => _.SubCategory.Category, _ => _.Brand, _ => _.ProductPhotos]);
            ThrowNotFoundIfProductNotExists(product);
            var mappedProduct = _mapper.Map<ProductListDto>(product);
            mappedProduct.Images = GetProductPhotos(product.ProductPhotos);

            return mappedProduct;
        }

        public async Task<ProductListDto> GetSingleProductAsync(Expression<Func<Product, bool>> predicate)
        {
            var product = await Products.GetSingleAsync(predicate, includeProperties: [_ => _.SubCategory, _ => _.SubCategory.Category, _ => _.Brand]);
            ThrowNotFoundIfProductNotExists(product);
            var mappedProduct = _mapper.Map<ProductListDto>(product);

            return mappedProduct;
        }

        public List<ProductListDto> GetProductsWhere(ProductRequestFilter filters, Expression<Func<Product, bool>> predicate)
        {
            var products = Products.GetWhere(predicate, includeProperties: [_ => _.SubCategory, _ => _.SubCategory.Category, _ => _.Brand]);
            var filteredProducts = new ProductFilterService(_mapper, products).FilterProducts(filters);
            new HeaderService(_httpContext).AddToHeaders(filteredProducts.Headers);

            return filteredProducts.ResponseValue;
        }

        public async Task<bool> IsCompaniesProduct(string productId, string companyId)
        {
            var product = await Products.GetSingleAsync(_ => _.Id == Guid.Parse(productId) && _.CreatedBy == Guid.Parse(companyId), false);
            return product != null;
        }

        private bool ThrowNotFoundIfProductNotExists(Product product)
            => product is null
                ? throw new NotFoundException("Ürün bulunamadı!")
                : true;

        private List<ProductPhotoListDto> GetProductPhotos(ICollection<ProductPhoto> productPhotos)
        {
            var images = new List<ProductPhotoListDto>();
            foreach (var photo in productPhotos)
                images.Add(new ProductPhotoListDto { Id = photo.Id, ImageBase64 = _fileService.GetImage(photo.PhotoPath) });

            return images;
        }
    }
}
