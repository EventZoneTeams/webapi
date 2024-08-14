using EventZone.Domain.DTOs.NotificationDTOs;
using EventZone.Domain.Entities;

namespace EventZone.Services.Interface
{
    public interface INotificationService
    {
        Task<List<NotificationDTO>> GetNotifications();

        Task<int> GetUnreadNotificationQuantity();

        Task PushNotification(Notification notification);

        Task PushNotificationToManager(Notification notification);

        Task<List<NotificationDTO>> ReadAllNotification();
    }
}