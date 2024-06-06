
using Microsoft.AspNetCore.Http;
using Repositories.Models.ImageDTOs;

namespace Services.Interface
{
    public interface IImageService
    {
        public Task<string> UploadImageAsync(IFormFile file, string folderName);
        public Task<List<ImageReturnDTO>> UploadMultipleImagesAsync(List<IFormFile> fileImages, string folderName);
        public Task<bool> DeleteImageAsync(string publicId);
        Task<List<ImageReturnDTO>> UploadImageRangeAsync(List<IFormFile> fileImages, string folderName);
    }
}
