using Domain.Enums;
using Services.DTO.EventFeedbackModel;
using Services.DTO.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IEventFeedbackService
    {
        Task<ResponseGenericModel<EventFeedbackDetailModel>> CreateFeedBackForEvent(CreateFeedbackModel inputFeedback, FeedbackTypeEnums type);

        Task<ResponseGenericModel<List<EventFeedbackDetailModel>>> DeleteFeedbacksAsync(List<int> feedbackIds);

        Task<List<EventFeedbackDetailModel>> GettAllFeedbacksAsync();

        Task<List<EventFeedbackDetailModel>> GettAllFeedbacksByEventIdAsync(int eventId);
    }
}