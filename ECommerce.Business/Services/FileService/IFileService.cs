using Microsoft.AspNetCore.Http;

namespace ECommerce.Business.Services.ImageService
{
    public interface IFileService
    {
        Task<string> UploadFile(string folderNameToUpload, IFormFile file);
        void RemoveFile(string filePath);
        string GetImage(string filePath);
    }
}
