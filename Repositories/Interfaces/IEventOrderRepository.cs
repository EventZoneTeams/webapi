using Domain.Entities;
using Domain.Enums;
using Repositories.Helper;

namespace Repositories.Interfaces
{
    public interface IEventOrderRepository : IGenericRepository<EventOrder>
    {
        Task<EventOrder> CreateOrderWithOrderDetails(int eventId, int userId, List<EventOrderDetail> orderDetails);
        Task<List<EventOrder>> getCurrentUserOrder();
        Task<EventOrder> UpdateOrderStatus(int orderId, EventOrderStatusEnums eventOrderStatusEnums);
        IQueryable<EventOrder> FilterAllField(int eventId, OrderParams orderParams);
    }
}
