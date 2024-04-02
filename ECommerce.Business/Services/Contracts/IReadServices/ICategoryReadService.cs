using ECommerce.Business.Helpers.Categories;
using ECommerce.Business.Models.Dtos.Categories;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Entity.Entities;
using System.Linq.Expressions;

namespace ECommerce.Business.Services.Contracts.IReadServices
{
    public interface ICategoryReadService
    {
        Task<CategoryListDto> GetCategoryByIdAsync(string categoryId);
        List<CategoryListDto> GetCategoriesWhere(CategoryRequestFilter filters, Expression<Func<Category, bool>> predicate);
        //SubCategoryListDto GetSubCategory(string subCategoryId);
        IReadRepository<Category> Categories { get; }
    }
}
