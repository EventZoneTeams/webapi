using Microsoft.AspNetCore.Mvc;
using Services.DTO.EmailModels;
using Services.DTO.TestModels;
using Services.Interface;
using System.Reflection;

namespace WebAPI.Controllers
{
    [Route("api/v1/test-upload")]
    [ApiController]
    public class TestUploadController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IEmailService _emailService;

        public TestUploadController(IImageService imageService, IEmailService emailService)
        {
            _imageService = imageService;
            _emailService = emailService;
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

        [HttpGet("test-send-html-email")]
        public async Task<IActionResult> TestSendHTMLImage()
        {
            try
            {
                var htmlContent = string.Empty;

                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", "Invoice", "index.html");

                using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        htmlContent = sr.ReadToEnd();
                    }
                }

                Message message = new Message
                (
                    ["lequocuyit@gmail.com"],
                    "Test send HTML email",
                    htmlContent
                );

                await _emailService.SendHTMLEmail(message);
                return Ok("Gui thanh cong");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}