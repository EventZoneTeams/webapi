using EventZone.Domain.Entities;

namespace EventZone.Repositories.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<int> GetUnreadNotificationQuantity();
        Task<List<Notification>> ReadAllNotification();
        Task<List<Notification>> GetListByUserId();
    }
}
