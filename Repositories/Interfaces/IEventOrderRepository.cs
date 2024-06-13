using Domain.Entities;
using Domain.Enums;

namespace Repositories.Interfaces
{
    public interface IEventOrderRepository : IGenericRepository<EventOrder>
    {
        Task<EventOrder> CreateOrderWithOrderDetails(int eventId, int userId, List<EventOrderDetail> orderDetails);
        Task<List<EventOrder>> GetEventOrdersByEventId(int eventId);
        Task<EventOrder> UpdateOrderStatus(int orderId, EventOrderStatusEnums eventOrderStatusEnums);
    }
}
