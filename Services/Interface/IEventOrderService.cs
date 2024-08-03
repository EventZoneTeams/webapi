using Domain.DTOs.EventOrderDTOs;
using Domain.Entities;
using Domain.Enums;
using Repositories.Helper;

namespace Services.Interface
{
    public interface IEventOrderService
    {
        Task<PagedList<EventOrder>> GetEventOrders(Guid eventId, OrderParams orderParams);

        Task<EventOrderReponseDTO> GetEventOrder(Guid id);

        Task<EventOrderReponseDTO> CreateEventOrder(CreateEventOrderReponseDTO order);

        Task<EventOrderReponseDTO> UpdateOrderStatus(Guid orderId, EventOrderStatusEnums eventOrderStatusEnums);
    }
}