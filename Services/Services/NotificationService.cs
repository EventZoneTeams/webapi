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

        public async Task PushNotification(Notification notification)
        {


            var newNotification = new Notification
            {
                Title = notification.Title,
                Body = notification.Body,
                UserId = notification.UserId,
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
                UserId = null,
                IsRead = false,
                Url = notification.Url,
                Sender = "Manager"
            };
            //save notification to DB
            await _unitOfWork.NotificationRepository.AddAsync(newNotification);
            await _unitOfWork.SaveChangeAsync();

            //push notification to signalR
            await _notificationHubContext.Clients.Group("Manager").SendAsync("ReceiveNotification", notification.Title, notification.Body).ConfigureAwait(true);
        }

        public async Task<List<NotificationDTO>> GetNotifications(Guid userId)
        {
            var notifications = await _unitOfWork.NotificationRepository.GetListByUserId(userId);
            return _mapper.Map<List<NotificationDTO>>(notifications);
        }

        public async Task<List<NotificationDTO>> ReadAllNotification(Guid userId)
        {
            var notifications = await _unitOfWork.NotificationRepository.ReadAllNotification(userId);
            return _mapper.Map<List<NotificationDTO>>(notifications);
        }
        public async Task<int> GetUnreadNotificationQuantity(Guid userId)
        {
            var result = await _unitOfWork.NotificationRepository.GetUnreadNotificationQuantity(userId);
            return result;
        }
    }
}
