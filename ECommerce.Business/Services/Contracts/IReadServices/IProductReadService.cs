using ECommerce.Business.Helpers.Products;
using ECommerce.Business.Models.Dtos.Products;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Entity.Entities;
using System.Linq.Expressions;

namespace ECommerce.Business.Services.Contracts.IReadServices
{
    public interface IProductReadService
    {
        List<ProductListDto> GetProductsWhere(ProductRequestFilter filters, Expression<Func<Product, bool>> predicate);
        Task<ProductListDto> GetProductByIdAsync(string productId);
        Task<ProductListDto> GetSingleProductAsync(Expression<Func<Product, bool>> predicate);
        Task<bool> IsCompaniesProduct(string productId, string companyId);
        IReadRepository<Product> Products { get; }
    }
}
