using Microsoft.AspNetCore.Http;

namespace EventZone.Services.DTO.TestModels
{
    public class UploadMultipleImage
    {
        public List<IFormFile> fileImages { get; set; }
        public List<string> OthersField { get; set; }
    }
}