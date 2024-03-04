using Microsoft.AspNetCore.Http;

namespace ECommerce.Business.Services.ImageService
{
    public interface IFileService
    {
        Task<string> UploadFile(string folderNameToUpload, IFormFile file);
        void RemoveFile(string folderName, string fileName);
        byte[] GetImage(string folderName, string fileName);
    }
}
