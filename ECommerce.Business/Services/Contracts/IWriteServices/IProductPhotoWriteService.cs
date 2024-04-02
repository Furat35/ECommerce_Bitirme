using ECommerce.Entity.Entities;

namespace ECommerce.Business.Services.Contracts.IWriteServices
{
    public interface IProductPhotoWriteService
    {
        Task<bool> RemoveProductPhoto(Product product, string imageId);
        Task RemoveProductPhotos(List<ProductPhoto> productPhotos);
        Task<bool> UploadProductPhoto(Product product);
        Task ThrowErrorIfImageLimitIsExceed(Product product);
    }
}
