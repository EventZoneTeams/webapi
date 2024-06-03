using Microsoft.AspNetCore.Http;

namespace Services.BusinessModels.TestModels
{
    public class UploadMultipleImage
    {
        public List<IFormFile> fileImages { get; set; }
        public string OthersField { get; set; }
    }
}
