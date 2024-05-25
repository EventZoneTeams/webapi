using Microsoft.AspNetCore.Http;

namespace Services.Interface
{
    public interface IImageService
    {
        public Task<string> UploadImageAsync(IFormFile file, string folderName);
        public Task<bool> DeleteImageAsync(string publicId);
    }
}
