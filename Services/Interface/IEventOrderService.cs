using Domain.Enums;
using Services.DTO.EventOrderDTOs;

namespace Services.Interface
{
    public interface IEventOrderService
    {
        Task<List<EventOrderReponseDTO>> GetEventOrders(int eventId);
        Task<EventOrderReponseDTO> GetEventOrder(int id);
        Task<EventOrderReponseDTO> CreateEventOrder(CreateEventOrderReponseDTO order);
        Task<EventOrderReponseDTO> UpdateOrderStatus(int orderId, EventOrderStatusEnums eventOrderStatusEnums);
    }
}
