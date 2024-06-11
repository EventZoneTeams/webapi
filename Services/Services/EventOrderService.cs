using Services.DTO.EventOrderDTOs;
using Services.Interface;

namespace Services.Services
{
    public class EventOrderService : IEventOrderService
    {
        public Task<EventOrderReponseDTO> GetEventOrder(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<EventOrderReponseDTO>> GetEventOrders()
        {
            throw new NotImplementedException();
        }
    }
}
