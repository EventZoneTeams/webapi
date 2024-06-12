using Services.DTO.EventOrderDTOs;

namespace Services.Interface
{
    public interface IEventOrderService
    {
        Task<List<EventOrderReponseDTO>> GetEventOrders();
        Task<EventOrderReponseDTO> GetEventOrder(int id);
        Task<EventOrderReponseDTO> CreateEventOrder(CreateEventOrderReponseDTO order);
    }
}
