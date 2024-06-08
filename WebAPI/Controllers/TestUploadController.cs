using Microsoft.AspNetCore.Mvc;
using Services.DTO.TestModels;
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

        [HttpPost("test-upload-multiple-ver-2")]
        public async Task<IActionResult> TestVer2([FromForm] UploadMultipleImage uploadMultipleImage)
        {
            if (uploadMultipleImage.fileImages == null || uploadMultipleImage.fileImages.Count == 0)
            {
                return BadRequest("No files were provided.");
            }

            var uploadedFileUrls = await _imageService.UploadImageRangeAsync(uploadMultipleImage.fileImages, "test-image-multiple");

            if (uploadedFileUrls.Count == 0)
            {
                return BadRequest("Failed to upload any files.");
            }

            return Ok(uploadedFileUrls);
        }
    }
}