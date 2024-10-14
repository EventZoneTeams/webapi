using AutoMapper;
using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using EventZone.Services.Interface;

namespace EventZone.Services.Services
{
    public class WithdrawnRequestService : IWithdrawnRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public WithdrawnRequestService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<WithdrawnRequest> CreateARequest(WithdrawnRequest request)
        {
            var result = await _unitOfWork.WithdrawnRequestRepository.CreateARequest(request);
            // Optionally send a notification for creating a new request
            //await _notificationService.SendNotificationAsync(request.UserId, "Your withdrawal request has been created and is pending approval.");
            return result;
        }

        public async Task<List<WithdrawnRequest>> GetRequestByUserId()
        {
            var result = await _unitOfWork.WithdrawnRequestRepository.GetRequestByUserId();
            return result;
        }

        public async Task<List<WithdrawnRequest>> GetAllRequest()
        {
            var result = await _unitOfWork.WithdrawnRequestRepository.GetAllRequest();
            return result;
        }

        public async Task<WithdrawnRequest> ApproveRequest(Guid requestId, string imageUrl)
        {
            var result = await _unitOfWork.WithdrawnRequestRepository.ApproveRequest(requestId, imageUrl);
            // Send notification to the user about approval
            //await _notificationService.SendNotificationAsync(result.UserId, $"Your withdrawal request has been approved. Amount: {result.Amount}");
            return result;
        }

        public async Task<WithdrawnRequest> RejectRequest(Guid requestId)
        {
            var result = await _unitOfWork.WithdrawnRequestRepository.RejectRequest(requestId);
            // Send notification to the user about rejection
            //await _notificationService.SendNotificationAsync(result.UserId, $"Your withdrawal request has been rejected.");
            return result;
        }
    }
}
