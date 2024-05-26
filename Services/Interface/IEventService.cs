using Services.BusinessModels.EventModels;

namespace Services.Interface
{
    public interface IEventService
    {
        Task<List<EventModel>> GetEvent();
        Task<EventModel> GetEventById(int id);
        Task<EventModel> CreateEvent(EventModel eventModel);
        Task<EventModel> UpdateEvent(int id, EventModel eventModel);
        Task<EventModel> DeleteEvent(int id);
    }
}
