using ECommerce.Business.Models.Dtos.Brands;

namespace ECommerce.Business.Services.Contracts.IWriteServices
{
    public interface IBrandWriteService
    {
        Task<BrandListDto> AddBrandAsync(BrandAddDto brand);
        Task<bool> SafeRemoveBrandAsync(string brandId);
        Task<bool> UpdateBrandAsync(BrandUpdateDto brand);
    }
}
