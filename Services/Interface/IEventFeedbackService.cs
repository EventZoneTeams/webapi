using Domain.Enums;
using Repositories.Commons;
using Services.DTO.EventFeedbackModel;

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