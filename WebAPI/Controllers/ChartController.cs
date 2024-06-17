using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Controllers
{
    [Route("api/v1/charts")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private IHubContext<DataHub> _hubContext;

        public ChartController(IHubContext<DataHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("send/graph1")]
        public async Task<IActionResult> SendGraph1()
        {
            Console.WriteLine("Send To Graph 1");
            await _hubContext.Clients.All.SendAsync("chartStation1", 5);

            return Ok(new { Status = "Send To Graph 1 Completed" });
        }

        [HttpGet("send/graph2")]
        public async Task<IActionResult> SendGraph2()
        {
            Console.WriteLine("Send To Graph 1");
            await _hubContext.Clients.All.SendAsync("chartStatus2", 10);

            return Ok(new { Status = "Send To Graph 2 Completed" });
        }
    }
}
