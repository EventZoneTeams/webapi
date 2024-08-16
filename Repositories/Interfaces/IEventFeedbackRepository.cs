using EventZone.Domain.Entities;

namespace EventZone.Repositories.Interfaces
{
    public interface IEventFeedbackRepository : IGenericRepository<EventFeedback>
    {
        Task<EventFeedback> CreateFeedbackAsync(EventFeedback newFeedback);

        Task<List<EventFeedback>> GettAllFeedbacksAsync();
    }
}