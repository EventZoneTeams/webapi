using AutoMapper;
using EventZone.Domain.DTOs.EventDonationDTOs;
using EventZone.Domain.Entities;
using EventZone.Domain.Enums;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Interfaces;
using EventZone.Services.Interface;

namespace EventZone.Services.Services
{
    public class EventDonationService : IEventDonationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly INotificationService _notificationService;
        private readonly IWalletService _walletService;

        public EventDonationService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService, INotificationService notificationService, IWalletService walletService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
            _notificationService = notificationService;
            _walletService = walletService;
        }

        public async Task<ApiResult<EventDonationDetailDTO>> AddDonationToCampaign(EventDonationCreateDTO data)
        {
            try
            {
                var checkEvent = await _unitOfWork.EventCampaignRepository.GetByIdAsync(data.EventCampaignId, x => x.Event);
                var checkUser = _claimsService.GetCurrentUserId == Guid.Empty ? Guid.Empty : _claimsService.GetCurrentUserId; //test

                if (checkEvent == null)
                {
                    return new ApiResult<EventDonationDetailDTO>()
                    {
                        IsSuccess = false,
                        Message = " This campaign is not existed",
                        Data = null
                    };
                }

                var newDonation = new EventDonation
                {
                    EventCampaignId = data.EventCampaignId,
                    UserId = checkUser,
                    Amount = data.Amount,
                    DonationDate = DateTime.UtcNow.AddHours(7)
                };

                checkEvent.CollectedAmount += newDonation.Amount;

                if (checkEvent.CollectedAmount > checkEvent.GoalAmount)
                {
                    return new ApiResult<EventDonationDetailDTO>()
                    {
                        IsSuccess = false,
                        Message = "added failed, cannot donate more than goal amount",
                        Data = _mapper.Map<EventDonationDetailDTO>(newDonation)
                    };
                }
                await _unitOfWork.EventCampaignRepository.Update(checkEvent);

                var newData = await _unitOfWork.EventDonationRepository.AddAsync(newDonation);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    // Send notification
                    var notification = new Notification
                    {
                        Title = "Donation Successfully!",
                        Body = "Amount: " + newDonation.Amount,
                        ReceiverId = newDonation.UserId,
                        Url = "/profile/wallets",
                        Sender = "System"
                    };
                    //await _notificationService.PushNotification(notification);

                    // Decrease money in wallet
                    var transation = await _unitOfWork.WalletRepository.Donation(newDonation.UserId, newDonation.Amount);

                    // Increase money of event owner
                    var eventOwnerWallet = await _unitOfWork.WalletRepository.GetWalletByUserIdAndType(checkEvent.Event.UserId, WalletTypeEnums.PERSONAL);
                    eventOwnerWallet.Balance += newDonation.Amount;

                    await _unitOfWork.WalletRepository.Update(eventOwnerWallet);
                    await _unitOfWork.SaveChangeAsync();

                    //create transaction for event owner
                    await _unitOfWork.WalletRepository.ReceiveDonation(checkEvent.Event.UserId, newDonation.Amount);
                    await _unitOfWork.SaveChangeAsync();

                    //Notification to event owner
                    //var notificationtoorganizer = new Notification
                    //{
                    //    Title = "one person donate event ",
                    //    Body = "amount: " + newDonation.Amount,
                    //    UserId = checkEvent.Event.UserId,
                    //    Url = "/dashboard/my-events/" + checkEvent.Event,
                    //    Sender = "system"
                    //};
                    //await _notificationService.PushNotification(notificationtoorganizer);

                    return new ApiResult<EventDonationDetailDTO>()
                    {
                        IsSuccess = true,
                        Message = "Successfully added",
                        Data = _mapper.Map<EventDonationDetailDTO>(newData)
                    };
                }
                else
                {
                    throw new Exception("Added failed please check");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EventDonationDetailDTO>> GetAllDonationOfCampaign(Guid id)
        {
            var result = await _unitOfWork.EventDonationRepository.GetAllDonationByCampaignId(id);
            return _mapper.Map<List<EventDonationDetailDTO>>(result);
        }

        public async Task<List<EventDonationDetailDTO>> GetMyDonation()
        {
            var userId = _claimsService.GetCurrentUserId;
            if (userId == Guid.Empty) throw new Exception("you are not login or bearer is not correct");
            var result = await _unitOfWork.EventDonationRepository.GetMyDonation();
            return _mapper.Map<List<EventDonationDetailDTO>>(result);
        }
    }
}