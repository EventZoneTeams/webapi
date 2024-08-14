using EventZone.Domain.DTOs.EventFeedbackDTOs;
using EventZone.Domain.Enums;
using EventZone.Repositories.Commons;

namespace EventZone.Services.Interface
{
    public interface IEventFeedbackService
    {
        Task<ApiResult<EventFeedbackDetailModel>> CreateFeedBackForEvent(CreateFeedbackModel inputFeedback, FeedbackTypeEnums type);

        Task<ApiResult<List<EventFeedbackDetailModel>>> DeleteFeedbacksAsync(List<Guid> feedbackIds);

        Task<List<EventFeedbackDetailModel>> GettAllFeedbacksAsync();

        Task<List<EventFeedbackDetailModel>> GettAllFeedbacksByEventIdAsync(Guid eventId);
    }
}