using ECommerce.Business.Models.Dtos.Categories;

namespace ECommerce.Business.Services.Categories.Abstract
{
    public interface ICategoryWriteService
    {
        Task<CategoryListDto> AddCategoryAsync(CategoryAddDto category);
        Task<bool> SafeRemoveCategoryAsync(string categoryId);
        Task<bool> UpdateCategoryAsync(CategoryUpdateDto category);
    }
}
