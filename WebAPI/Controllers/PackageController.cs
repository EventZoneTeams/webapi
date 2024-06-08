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
        public async Task<IActionResult> TestAsync(int eventId, [FromForm] test input)
        {
            try
            {
                if (input.product.Count == 0)
                {
                    return BadRequest(input);
                }
                return Ok(input);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class test
    {
        [ModelBinder(BinderType = typeof(DtoFormBinder))]
        public List<EventProductDetailDTO> product { get; set; } = null;

        public List<string>? properties { get; set; } = null;
    }

    public class EventProductDetailDTO
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}