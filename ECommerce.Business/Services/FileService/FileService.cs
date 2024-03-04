using Microsoft.AspNetCore.Http;
namespace ECommerce.Business.Services.ImageService
{
    public class FileService : IFileService
    {
        public async Task<string> UploadFile(string folderNameToUpload, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Geçersiz dosya");

            var currentDirectory = Directory.GetCurrentDirectory();
            var uploadsFolder = Path.Combine(currentDirectory, "uploads", folderNameToUpload);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }

        public void RemoveFile(string folderName, string fileName)
        {
            if (string.IsNullOrEmpty(folderName) || string.IsNullOrEmpty(fileName))
                throw new ArgumentException("Geçersiz dosya adı");

            var currentDirectory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(currentDirectory, "uploads", folderName);

            if (File.Exists(filePath))
                File.Delete(filePath);
            else
                throw new FileNotFoundException($"Dosya bulunamadı: {filePath}");
        }

        public byte[] GetImage(string folderName, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("Invalid file name");
            }

            var currentDirectory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(currentDirectory, "uploads", folderName);

            if (File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }
            else
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }
        }
    }
}
