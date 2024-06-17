using Services.DTO.NotificationDTOs;

namespace Services.Interface
{
    public interface INotificationService
    {
        Task<List<NotificationDTO>> GetNotifications(int userId);
        Task<int> GetUnreadNotificationQuantity(int userId);
        Task PushNotification(int userId, string title, string body);
        Task<List<NotificationDTO>> ReadAllNotification(int userId);
    }
}
