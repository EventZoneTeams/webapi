using Domain.Entities;
using Services.DTO.NotificationDTOs;

namespace Services.Interface
{
    public interface INotificationService
    {
        Task<List<NotificationDTO>> GetNotifications(int userId);
      
        Task<int> GetUnreadNotificationQuantity(int userId);
      
        Task PushNotification(Notification notification);
      
        Task PushNotificationToManager(Notification notification);
      
        Task<List<NotificationDTO>> ReadAllNotification(int userId);
    }
}