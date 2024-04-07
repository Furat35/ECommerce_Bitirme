using ECommerce.Business.Models.Dtos.Products;

namespace ECommerce.Business.Services.Contracts.IWriteServices
{
    public interface IProductWriteService
    {
        Task<ProductListDto> AddProductAsync(ProductAddDto product);
        Task SafeRemoveProductRangeAsync(List<string> productIds);
        Task<bool> SafeRemoveProductAsync(string productId);
        Task<bool> RemoveProductPhoto(string productId, string imageId);
        Task<bool> UpdateProductAsync(ProductUpdateDto product);
        Task<bool> UploadProductPhoto(string productId);
        Task<bool> DecreaseProductQuantity(string productId, int quantity);
        Task<bool> ConfirmProductToAdded(string productId);
        Task<bool> ConfirmAllProductsToAdded();
    }
}
