using EventZone.Domain.Entities;
using EventZone.Domain.Enums;
using EventZone.Repositories.Helper;

namespace EventZone.Repositories.Interfaces
{
    public interface IEventOrderRepository : IGenericRepository<EventOrder>
    {
        Task<EventOrder> CreateOrderWithOrderDetails(Guid eventId, Guid userId, List<EventOrderDetail> orderDetails);
        Task<List<EventOrder>> getCurrentUserOrder();
        Task<EventOrder> UpdateOrderStatus(Guid orderId, EventOrderStatusEnums eventOrderStatusEnums);
        IQueryable<EventOrder> FilterAllField(Guid eventId, OrderParams orderParams);
        Task<EventOrderDetail> CheckInStatusProduct(Guid id);
    }
}
