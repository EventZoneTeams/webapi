using Domain.Entities;
using Domain.Enums;
using Repositories.Helper;
using Services.DTO.EventOrderDTOs;

namespace Services.Interface
{
    public interface IEventOrderService
    {
        Task<PagedList<EventOrder>> GetEventOrders(int eventId, OrderParams orderParams);
        Task<EventOrderReponseDTO> GetEventOrder(int id);
        Task<EventOrderReponseDTO> CreateEventOrder(CreateEventOrderReponseDTO order);
        Task<EventOrderReponseDTO> UpdateOrderStatus(int orderId, EventOrderStatusEnums eventOrderStatusEnums);
    }
}
