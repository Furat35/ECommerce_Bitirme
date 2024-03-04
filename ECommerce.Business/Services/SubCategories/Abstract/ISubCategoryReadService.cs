using ECommerce.Business.Helpers.SubCategories;
using ECommerce.Business.Models.Dtos.SubCategories;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Entity.Entities;
using System.Linq.Expressions;

namespace ECommerce.Business.Services.SubCategories.Abstract
{
    public interface ISubCategoryReadService
    {
        Task<SubCategoryListDto> GetSubCategoryByIdAsync(string subCategoryId);
        List<SubCategoryListDto> GetSubCategoriesWhere(SubCategoryRequestFilter filters, Expression<Func<SubCategory, bool>> predicate);
        IReadRepository<SubCategory> SubCategories { get; }
    }
}
