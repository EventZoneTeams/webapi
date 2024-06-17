using AutoMapper;
using Repositories.Interfaces;
using Services.DTO.NotificationDTOs;
using Services.Interface;

namespace Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
