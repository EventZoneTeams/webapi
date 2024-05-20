using Repositories.DTO;

namespace Services.Interface
{
    public interface IEventService
    {
        Task<List<EventModel>> GetEventsAsync();
        Task<EventModel> GetEventByIdAsync(int id);
        //Task<EventModel> AddEventAsync(EventModel eventModel);
        //Task<EventModel> UpdateEventAsync(int id, EventModel eventModel);
        ///Task<bool> DeleteEventAsync(int id);
    }
}
