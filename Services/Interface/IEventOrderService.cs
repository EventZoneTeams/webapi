using EventZone.Domain.DTOs.EventOrderDTOs;
using EventZone.Domain.Entities;
using EventZone.Domain.Enums;
using EventZone.Repositories.Helper;

namespace EventZone.Services.Interface
{
    public interface IEventOrderService
    {
        Task<PagedList<EventOrder>> GetEventOrders(Guid eventId, OrderParams orderParams);

        Task<EventOrderReponseDTO> GetEventOrder(Guid id);

        Task<EventOrderReponseDTO> CreateEventOrder(CreateEventOrderReponseDTO order);

        Task<EventOrderReponseDTO> UpdateOrderStatus(Guid orderId, EventOrderStatusEnums eventOrderStatusEnums);
    }
}