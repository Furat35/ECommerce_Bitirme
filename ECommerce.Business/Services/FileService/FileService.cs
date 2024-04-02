﻿using Microsoft.AspNetCore.Http;
namespace ECommerce.Business.Services.ImageService
{
    public class FileService : IFileService
    {
        public async Task<string> UploadFile(string folderNameToUpload, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Geçersiz dosya");

            var currentDirectory = Directory.GetCurrentDirectory();
            var uploadsFolder = Path.Combine(currentDirectory, "wwwroot", "uploads", folderNameToUpload);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Path.Combine("wwwroot", "uploads", folderNameToUpload, uniqueFileName);
        }

        public void RemoveFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(filePath))
                throw new ArgumentException("Geçersiz dosya adı");

            var currentDirectory = Directory.GetCurrentDirectory();
            var fileToDelete = Path.Combine(currentDirectory, filePath);

            if (File.Exists(fileToDelete))
                File.Delete(fileToDelete);
            else
                throw new FileNotFoundException($"Dosya bulunamadı: {filePath}");
        }

        public string GetImage(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Invalid file name");
            }

            var currentDirectory = Directory.GetCurrentDirectory();
            var fileToGet = Path.Combine(currentDirectory, filePath);

            if (File.Exists(fileToGet))
            {
                return Convert.ToBase64String(File.ReadAllBytes(fileToGet));
            }
            else
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }
        }
    }
}
