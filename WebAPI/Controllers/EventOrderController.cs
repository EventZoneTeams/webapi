using Microsoft.AspNetCore.Mvc;
using Services.DTO.EventOrderDTOs;
using Services.Interface;

namespace WebAPI.Controllers
{
    [Route("api/v1/event-orders")]
    [ApiController]
    public class EventOrderController : Controller
    {
        private readonly IEventOrderService _eventOrderService;

        public EventOrderController(IEventOrderService eventOrderService)
        {
            _eventOrderService = eventOrderService;
        }

        // GET: api/v1/event-orders
        // <summary>
        // Get list event orders
        // </summary>
        [HttpGet]
        public async Task<ActionResult<List<EventOrderReponseDTO>>> GetEventOrders()
        {
            return await _eventOrderService.GetEventOrders();
        }

        // GET: api/v1/event-orders/5
        // <summary>
        // Get event order by id
        // </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<EventOrderReponseDTO>> GetEventOrder(int id)
        {
            return await _eventOrderService.GetEventOrder(id);
        }

    }
}
