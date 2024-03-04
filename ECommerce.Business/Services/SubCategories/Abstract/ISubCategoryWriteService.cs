using ECommerce.Business.Models.Dtos.SubCategories;

namespace ECommerce.Business.Services.SubCategories.Abstract
{
    public interface ISubCategoryWriteService
    {
        Task<SubCategoryListDto> AddSubCategoryAsync(SubCategoryAddDto subCategory);
        Task<bool> SafeRemoveSubCategoryAsync(string subCategoryId);
        Task<bool> UpdateSubCategoryAsync(SubCategoryUpdateDto subCategory);
    }
}
