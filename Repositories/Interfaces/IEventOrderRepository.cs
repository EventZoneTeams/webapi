using Domain.Entities;

namespace Repositories.Interfaces
{
    public interface IEventOrderRepository : IGenericRepository<EventOrder>
    {
        Task<EventOrder> CreateOrderWithOrderDetails(int eventId, int userId, List<EventOrderDetail> orderDetails);
    }
}
