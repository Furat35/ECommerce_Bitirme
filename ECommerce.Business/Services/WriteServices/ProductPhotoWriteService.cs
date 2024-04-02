using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Business.Services.ImageService;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Business.Services.WriteServices
{
    public class ProductPhotoWriteService : IProductPhotoWriteService
    {
        private readonly IWriteRepository<ProductPhoto> _productPhotoWriteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;

        public ProductPhotoWriteService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IFileService fileService)
        {
            _productPhotoWriteRepository = unitOfWork.GetWriteRepository<ProductPhoto>();
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
        }

        public async Task<bool> RemoveProductPhoto(Product product, string imageId)
        {
            var image = product.ProductPhotos.FirstOrDefault(_ => _.Id == Guid.Parse(imageId));
            _fileService.RemoveFile(image.PhotoPath);

            return image != null ? await _productPhotoWriteRepository.RemoveAsync(image) : false;
        }

        public async Task RemoveProductPhotos(List<ProductPhoto> productPhotos)
        {
            if (productPhotos.Count > 0)
            {
                await _productPhotoWriteRepository.RemoveRangeAsync(productPhotos);
                foreach (var productPhoto in productPhotos)
                    _fileService.RemoveFile(productPhoto.PhotoPath);
            }
        }

        public async Task<bool> UploadProductPhoto(Product product)
        {
            var imageToUpload = _httpContextAccessor.HttpContext.Request.Form.Files.FirstOrDefault();
            if (imageToUpload != null)
            {
                string photoPath = await _fileService.UploadFile("productimages", imageToUpload);
                var productPhoto = new ProductPhoto { PhotoPath = photoPath, ProductId = product.Id };

                return await _productPhotoWriteRepository.AddAsync(productPhoto);
            }

            return false;
        }

        public async Task ThrowErrorIfImageLimitIsExceed(Product product)
        {
            if (product.ProductPhotos.Count > 2)
                throw new BadRequestException("En fazla 3 görsel eklenebilmektedir!");
        }
    }
}
