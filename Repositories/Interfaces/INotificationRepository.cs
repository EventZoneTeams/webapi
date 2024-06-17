using Domain.Entities;

namespace Repositories.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<int> GetUnreadNotificationQuantity(int userId);
        Task<List<Notification>> ReadAllNotification(int userId);
    }
}
