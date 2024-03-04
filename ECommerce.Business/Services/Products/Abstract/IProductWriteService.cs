using ECommerce.Business.Models.Dtos.Products;

namespace ECommerce.Business.Services.Products.Abstract
{
    public interface IProductWriteService
    {
        Task<ProductListDto> AddProductAsync(ProductAddDto product);
        Task<bool> SafeRemoveProductRangeAsync(List<string> productIds);
        Task<bool> SafeRemoveProductAsync(string productId);
        Task<bool> UpdateProductAsync(ProductUpdateDto product);
        Task<bool> IsProductCreatedByUserAsync(string productId, string userId);
    }
}
