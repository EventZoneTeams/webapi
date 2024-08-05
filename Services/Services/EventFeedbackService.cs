using AutoMapper;
using Domain.DTOs.EventFeedbackDTOs;
using Domain.Entities;
using Domain.Enums;
using Repositories.Commons;
using Repositories.Interfaces;
using Services.Interface;

namespace Services.Services
{
    public class EventFeedbackService : IEventFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public EventFeedbackService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<ApiResult<EventFeedbackDetailModel>> CreateFeedBackForEvent(CreateFeedbackModel inputFeedback, FeedbackTypeEnums type)
        {
            try
            {
                var checkEvent = await _unitOfWork.EventRepository.GetByIdAsync(inputFeedback.EventId);
                if (checkEvent == null)
                {
                    return new ApiResult<EventFeedbackDetailModel>
                    {
                        Success = false,
                        Data = null,
                        Message = "This event is no longer existing, please try again"
                    };
                }

                EventFeedback newFeedback = new EventFeedback
                {
                    EventId = inputFeedback.EventId,
                    Content = inputFeedback.Content,
                };

                newFeedback = await _unitOfWork.EventFeedbackRepository.CreateFeedbackAsync(newFeedback);

                Notification notification = new Notification
                {
                    Body = "You have a new feedback: " + newFeedback.Content,
                    Title = "Feedback",
                    UserId = checkEvent.UserId,
                    IsRead = false,
                    Url = "/event/" + checkEvent.Id,
                    Sender = "System"
                };

                if (checkEvent.Status == EventStatusEnums.PENDING.ToString())
                {
                    switch (type)
                    {
                        case FeedbackTypeEnums.APPROVE:
                            checkEvent.Status = EventStatusEnums.APPROVED.ToString();

                            //if (checkEvent.IsDonation)
                            //{
                            //    checkEvent.Status = EventStatusEnums.DONATING.ToString();

                            //    notification.Title = "Your event is approved" + "(Event Id = " + checkEvent.Id + ")";
                            //    notification.Body = "Your event is approved, please check your event for more information";
                            //}
                            //else
                            //{
                            //    checkEvent.Status = EventStatusEnums.SUCCESSFUL.ToString();
                            //    notification.Title = "Your event is successful" + "(Event Id = " + checkEvent.Id + ")";
                            //    notification.Body = "Your event is successful, please check your event for more information";
                            //}

                            break;

                        case FeedbackTypeEnums.REJECT:
                            checkEvent.Status = EventStatusEnums.REJECTED.ToString();
                            notification.Title = "Your event is rejected" + "(Event Id = " + checkEvent.Id + ")";
                            notification.Body = "Your event is rejected: " + newFeedback.Content;
                            checkEvent.Status = EventStatusEnums.REJECTED.ToString();

                            break;

                        default:
                            if (Enum.IsDefined(typeof(FeedbackTypeEnums), type))
                            {
                                throw new Exception("trôn");
                            }
                            break;
                    }

                    bool updateStatus = await _unitOfWork.EventRepository.Update(checkEvent);

                    var saveCheck = await _unitOfWork.SaveChangeAsync();
                    if (updateStatus || saveCheck > 0)
                    {
                        var result = _mapper.Map<EventFeedbackDetailModel>(newFeedback);
                        await _notificationService.PushNotification(notification);
                        //  result.FeedbackType = type.ToString();
                        return new ApiResult<EventFeedbackDetailModel>
                        {
                            Success = true,
                            Data = result,
                            Message = "Added and updated status event successfully"
                        };
                    }
                }

                return new ApiResult<EventFeedbackDetailModel>
                {
                    Success = false,
                    Data = _mapper.Map<EventFeedbackDetailModel>(checkEvent),
                    Message = "This event have already feedback"
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EventFeedbackDetailModel>> GettAllFeedbacksAsync()
        {
            try
            {
                var data = _mapper.Map<List<EventFeedbackDetailModel>>(await _unitOfWork.EventFeedbackRepository.GettAllFeedbacksAsync());
                return data;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<List<EventFeedbackDetailModel>> GettAllFeedbacksByEventIdAsync(Guid eventId)
        {
            try
            {
                var data = _mapper.Map<List<EventFeedbackDetailModel>>(await _unitOfWork.EventFeedbackRepository.GettAllFeedbacksAsync());
                return data.Where(x => x.EventId == eventId).ToList();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<ApiResult<List<EventFeedbackDetailModel>>> DeleteFeedbacksAsync(List<Guid> feedbackIds)
        {
            var allFeedbacks = await _unitOfWork.EventFeedbackRepository.GetAllAsync();
            var existingIds = allFeedbacks.Where(e => feedbackIds.Contains(e.Id)).Select(e => e.Id).ToList();
            var nonExistingIds = feedbackIds.Except(existingIds).ToList();

            if (existingIds.Count > 0)
            {
                var result = await _unitOfWork.EventFeedbackRepository.SoftRemoveRangeById(existingIds);
                if (result)
                {
                    return new ApiResult<List<EventFeedbackDetailModel>>()
                    {
                        Success = true,
                        Message = " Added successfully",
                        Data = _mapper.Map<List<EventFeedbackDetailModel>>(allFeedbacks.Where(e => existingIds.Contains(e.Id)))
                    };
                }
            }
            else
            {
                if (nonExistingIds.Count > 0)
                {
                    string nonExistingIdsString = string.Join(", ", nonExistingIds);

                    return new ApiResult<List<EventFeedbackDetailModel>>()
                    {
                        Success = false,
                        Message = "There are few ids that are no existed product id: " + nonExistingIdsString,
                        Data = _mapper.Map<List<EventFeedbackDetailModel>>(allFeedbacks.Where(e => existingIds.Contains(e.Id)))
                    };
                }
            }
            return new ApiResult<List<EventFeedbackDetailModel>>()
            {
                Success = false,
                Message = "failed",
                Data = null
            };
        }
    }
}