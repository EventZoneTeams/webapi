using Services.BusinessModels.EventModels;

namespace Services.Interface
{
    public interface IEventService
    {
        Task<List<ResponseEventModel>> GetEvent();
        Task<ResponseEventModel> GetEventById(int id);
        Task<ResponseEventModel> CreateEvent(EventModel eventModel);
        Task<ResponseEventModel> UpdateEvent(int id, EventModel eventModel);
        Task<ResponseEventModel> DeleteEvent(int id);
    }
}
