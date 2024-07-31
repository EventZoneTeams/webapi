using Domain.Entities;

namespace Repositories.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<int> GetUnreadNotificationQuantity(Guid userId);
        Task<List<Notification>> ReadAllNotification(Guid userId);
        Task<List<Notification>> GetListByUserId(Guid userId);
    }
}
