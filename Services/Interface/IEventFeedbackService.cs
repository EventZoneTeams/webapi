using Domain.DTOs.EventFeedbackDTOs;
using Domain.Enums;
using Repositories.Commons;

namespace Services.Interface
{
    public interface IEventFeedbackService
    {
        Task<ApiResult<EventFeedbackDetailModel>> CreateFeedBackForEvent(CreateFeedbackModel inputFeedback, FeedbackTypeEnums type);

        Task<ApiResult<List<EventFeedbackDetailModel>>> DeleteFeedbacksAsync(List<Guid> feedbackIds);

        Task<List<EventFeedbackDetailModel>> GettAllFeedbacksAsync();

        Task<List<EventFeedbackDetailModel>> GettAllFeedbacksByEventIdAsync(Guid eventId);
    }
}