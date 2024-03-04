using AutoMapper;
using ECommerce.Business.Helpers.FilterServices;
using ECommerce.Business.Helpers.HeaderServices;
using ECommerce.Business.Helpers.Products;
using ECommerce.Business.Models.Dtos.Products;
using ECommerce.Business.Services.Products.Abstract;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace ECommerce.Business.Services.Products
{
    public class ProductReadService : IProductReadService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        public ProductReadService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            Products = unitOfWork.GetReadRepository<Product>();
            _mapper = mapper;
            _httpContext = contextAccessor;
        }

        public IReadRepository<Product> Products { get; }

        public async Task<ProductListDto> GetProductIdAsync(string productId)
        {
            var product = await Products.GetByIdAsync(productId, includeProperties: [_ => _.SubCategory, _ => _.SubCategory.Category, _ => _.Brand]);
            ThrowNotFoundIfProductNotExists(product);
            var mappedProduct = _mapper.Map<ProductListDto>(product);

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

        private bool ThrowNotFoundIfProductNotExists(Product product)
            => product is null
                ? throw new NotFoundException("Ürün bulunamadı!")
                : true;
    }
}
