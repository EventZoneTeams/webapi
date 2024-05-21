using Repositories.DTO;
using Services.ViewModels.EventModels;

namespace Services.Interface
{
    public interface IEventService
    {
        Task<List<EventModel>> GetEventsAsync();
        Task<EventModel> GetEventByIdAsync(int id);
        Task<EventModel> CreateEventAsync(CreateEventModel eventModel);
        //Task<EventModel> UpdateEventAsync(int id, EventModel eventModel);
        ///Task<bool> DeleteEventAsync(int id);
    }
}
