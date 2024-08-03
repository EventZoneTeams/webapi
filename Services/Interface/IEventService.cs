using Domain.DTOs.EventDTOs;
using Domain.Entities;
using Repositories.Helper;

namespace Services.Interface
{
    public interface IEventService
    {
        Task<PagedList<Event>> GetEvent(EventParams eventParams);

        Task<EventResponseDTO> GetEventById(Guid id);

        Task<EventResponseDTO> CreateEvent(EventDTO eventModel);

        Task<EventResponseDTO> UpdateEvent(Guid id, EventDTO eventModel);

        Task<EventResponseDTO> DeleteEvent(Guid id);
    }
}