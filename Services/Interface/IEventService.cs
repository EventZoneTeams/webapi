using Domain.Entities;
using Repositories.Helper;
using Services.DTO.EventModels;

namespace Services.Interface
{
    public interface IEventService
    {
        Task<PagedList<Event>> GetEvent(EventParams eventParams);
        Task<ResponseEventModel> GetEventById(int id);
        Task<ResponseEventModel> CreateEvent(EventModel eventModel);
        Task<ResponseEventModel> UpdateEvent(int id, EventModel eventModel);
        Task<ResponseEventModel> DeleteEvent(int id);
    }
}
