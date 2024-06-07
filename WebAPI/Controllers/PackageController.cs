using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;
using System.Xml.Serialization;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        [HttpPost("lmao")]
        public async Task<IActionResult> CreatedqwdAsync(int eventId, [FromForm] test lqwdjqdwn)
        {
            try
            {
                return BadRequest(lqwdjqdwn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class test
    {
        public List<EventProductDetailDTO> product { get; set; } = null;
        public List<string>? properties { get; set; } = null;
    }

    public class Image
    {
        public IFormFile? image { get; set; }
    }
}