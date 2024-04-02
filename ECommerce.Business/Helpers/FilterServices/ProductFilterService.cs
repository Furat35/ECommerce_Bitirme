using AutoMapper;
using ECommerce.Business.Helpers.Products;
using ECommerce.Business.Models.Dtos.Products;
using ECommerce.Core.Filters;
using ECommerce.Core.ResponseHeaders;
using ECommerce.Entity.Entities;
using System.Linq.Expressions;

namespace ECommerce.Business.Helpers.FilterServices
{
    public class ProductFilterService
    {
        private readonly IMapper _mapper;
        private IQueryable<Product> _products;

        public ProductFilterService(IMapper mapper, IQueryable<Product> products)
        {
            _mapper = mapper;
            _products = products;
        }

        public ProductResponse<List<ProductListDto>> FilterProducts(ProductRequestFilter filters)
        {
            _products = NameStartsWith(filters.ProductName);
            if (filters.GreaterThan.HasValue)
                _products = PriceIsGreateThan(filters.GreaterThan.Value);
            if (filters.LessThan.HasValue)
                _products = PriceIsLessThan(filters.LessThan.Value);
            if (filters.SubCategory.HasValue)
                _products = GetBySubCategory(filters.SubCategory.Value);

            int pageNumber;
            if (_products.Count() > filters.PageSize)
                pageNumber = _products.Count() % filters.PageSize == 0 ? _products.Count() / filters.PageSize : _products.Count() / filters.PageSize + 1;
            else
                pageNumber = 1;
            Metadata metadata = new(filters.Page, filters.PageSize, _products.Count(), pageNumber);
            _products = AddPagination(filters);
            var header = new CustomHeaders().AddPaginationHeader(metadata);
            var mappedProducts = _mapper.Map<List<ProductListDto>>(_products);

            return new()
            {
                ResponseValue = mappedProducts,
                Headers = header
            };
        }

        private IQueryable<Product> NameStartsWith(string productName)
            => !string.IsNullOrEmpty(productName)
               ? _products.Where(product => product.ProductName.StartsWith(productName))
               : _products;

        private IQueryable<Product> PriceIsGreateThan(float price)
            => price > 0
                ? _products.Where(p => p.Price >= price)
                : _products;

        private IQueryable<Product> PriceIsLessThan(float price)
          => price > 0
              ? _products.Where(p => p.Price <= price)
              : _products;

        private IQueryable<Product> AddPagination(ProductRequestFilter filters)
        {
            string orderByProperty = filters.OrderBy.ToLower() ?? "productname";
            bool isDescending = filters.Order.ToLower() == "desc";

            var propertyInfo = typeof(Product).GetProperties()
             .FirstOrDefault(p => p.Name.Equals(orderByProperty, StringComparison.OrdinalIgnoreCase));

            if (propertyInfo != null)
            {
                var parameter = Expression.Parameter(typeof(Product), "x");
                var property = Expression.Property(parameter, propertyInfo);
                var conversion = Expression.Convert(property, typeof(object));
                var lambda = Expression.Lambda<Func<Product, object>>(conversion, parameter);

                _products = isDescending ? _products.OrderByDescending(lambda) : _products.OrderBy(lambda);
            }
            else
            {
                throw new ArgumentException("Geçersiz filtre!");
            }


            return _products
                .Skip(filters.Page * filters.PageSize)
                .Take(filters.PageSize); ;
        }


        private IQueryable<Product> GetBySubCategory(Guid subCategory)
            => _products
                .Where(p => p.SubCategoryId == subCategory);
    }
}
