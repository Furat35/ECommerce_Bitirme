using ECommerce.Business.Helpers.Brands;
using ECommerce.Business.Models.Dtos.Brands;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Entity.Entities;
using System.Linq.Expressions;

namespace ECommerce.Business.Services.Brands.Abstract
{
    public interface IBrandReadService
    {
        Task<BrandListDto> GetBrandByIdAsync(string productId);
        List<BrandListDto> GetBrandsWhere(BrandRequestFilter filters, Expression<Func<Brand, bool>> predicate);
        IReadRepository<Brand> Brands { get; }
    }
}
