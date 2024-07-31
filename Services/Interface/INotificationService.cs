using Domain.Entities;
using Services.DTO.NotificationDTOs;

namespace Services.Interface
{
    public interface INotificationService
    {
        Task<List<NotificationDTO>> GetNotifications(Guid userId);

        Task<int> GetUnreadNotificationQuantity(Guid userId);

        Task PushNotification(Notification notification);

        Task PushNotificationToManager(Notification notification);

        Task<List<NotificationDTO>> ReadAllNotification(Guid userId);
    }
}