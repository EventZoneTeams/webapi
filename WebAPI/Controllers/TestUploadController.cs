using Microsoft.AspNetCore.Mvc;
using Services.BusinessModels.TestModels;
using Services.Interface;

namespace WebAPI.Controllers
{
    [Route("api/v1/test-upload")]
    [ApiController]
    public class TestUploadController : Controller
    {
        private readonly IImageService _imageService;

        public TestUploadController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("test-upload-multiple")]
        public async Task<IActionResult> Test([FromForm] UploadMultipleImage uploadMultipleImage)
        {


            if (uploadMultipleImage.fileImages == null || uploadMultipleImage.fileImages.Count == 0)
            {
                return BadRequest("No files were provided.");
            }

            var uploadedFileUrls = await _imageService.UploadMultipleImagesAsync(uploadMultipleImage.fileImages, "test-image-multiple");

            if (uploadedFileUrls.Count == 0)
            {
                return BadRequest("Failed to upload any files.");
            }

            return Ok(uploadedFileUrls);
        }
    }
}
