using AutoMapper;
using EventZone.Domain.DTOs.NotificationDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using EventZone.Services.Hubs;
using EventZone.Services.Interface;
using Microsoft.AspNetCore.SignalR;

namespace EventZone.Services.Services
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

        public async Task PushNotification(Notification notification)
        {
            var newNotification = new Notification
            {
                Title = notification.Title,
                Body = notification.Body,
                ReceiverId = notification.ReceiverId,
                IsRead = false,
                Url = notification.Url,
                Sender = notification.Sender ?? "Admin"
            };
            //save notification to DB
            await _unitOfWork.NotificationRepository.AddAsync(newNotification);
            await _unitOfWork.SaveChangeAsync();

            //push notification to signalR
            await _notificationHubContext.Clients.All.SendAsync("ReceiveNotification", notification.Title, notification.Body).ConfigureAwait(true);


        }

        public async Task PushNotificationToManager(Notification notification)
        {
            var newNotification = new Notification
            {
                Title = notification.Title,
                Body = notification.Body,
                ReceiverId = null,
                IsRead = false,
                Url = notification.Url,
                Sender = "System"
            };
            //save notification to DB
            await _unitOfWork.NotificationRepository.AddAsync(newNotification);
            await _unitOfWork.SaveChangeAsync();

            //push notification to signalR
            await _notificationHubContext.Clients.Group("Manager").SendAsync("ReceiveNotification", notification.Title, notification.Body).ConfigureAwait(true);
        }

        public async Task<List<NotificationDTO>> GetNotifications()
        {
            var notifications = await _unitOfWork.NotificationRepository.GetListByUserId();
            return _mapper.Map<List<NotificationDTO>>(notifications);
        }

        public async Task<List<NotificationDTO>> ReadAllNotification()
        {
            var notifications = await _unitOfWork.NotificationRepository.ReadAllNotification();
            return _mapper.Map<List<NotificationDTO>>(notifications);
        }
        public async Task<int> GetUnreadNotificationQuantity()
        {
            var result = await _unitOfWork.NotificationRepository.GetUnreadNotificationQuantity();
            return result;
        }
    }
}
