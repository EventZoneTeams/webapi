using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Repositories.Interfaces;
using Services.DTO.NotificationDTOs;
using Services.Hubs;
using Services.Interface;

namespace Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _notificationHubContext;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<NotificationHub> notificationHubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationHubContext = notificationHubContext;
        }

        public async Task PushNotification(int userId, string title, string body)
        {
            var newNotification = new Notification
            {
                Title = title,
                Body = body,
                UserId = userId,
                IsRead = false,
                Url = null,
                Sender = "System"
            };
            //save notification to DB
            await _unitOfWork.NotificationRepository.AddAsync(newNotification);
            await _unitOfWork.SaveChangeAsync();

            //push notification to signalR
            await _notificationHubContext.Clients.All.SendAsync("sendToUser", title, body);
        }


        public async Task<List<NotificationDTO>> GetNotifications(int userId)
        {
            var notifications = await _unitOfWork.NotificationRepository.GetAllAsync(x => x.UserId == userId);
            return _mapper.Map<List<NotificationDTO>>(notifications);
        }

        public async Task<List<NotificationDTO>> ReadAllNotification(int userId)
        {
            var notifications = await _unitOfWork.NotificationRepository.ReadAllNotification(userId);
            return _mapper.Map<List<NotificationDTO>>(notifications);
        }
        public async Task<int> GetUnreadNotificationQuantity(int userId)
        {
            var result = await _unitOfWork.NotificationRepository.GetUnreadNotificationQuantity(userId);
            return result;
        }
    }
}
