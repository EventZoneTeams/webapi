using Microsoft.AspNetCore.Mvc;
using Repositories.Commons;
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
            try
            {
                return await _eventOrderService.GetEventOrders();
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        // GET: api/v1/event-orders/5
        // <summary>
        // Get event order by id
        // </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<EventOrderReponseDTO>> GetEventOrder(int id)
        {
            try
            {
                return await _eventOrderService.GetEventOrder(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        // POST: api/v1/event-orders
        // <summary>
        // Create event order
        // </summary>
        [HttpPost]
        public async Task<ActionResult<EventOrderReponseDTO>> CreateEventOrder(CreateEventOrderReponseDTO order)
        {
            try
            {
                return await _eventOrderService.CreateEventOrder(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

    }
}
