using EventZone.Domain.DTOs.EventDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Helper;

namespace EventZone.Services.Interface
{
    public interface IEventService
    {
        Task<PagedList<Event>> GetEvent(EventParams eventParams);

        Task<EventResponseDTO> GetEventById(Guid id);

        Task<EventResponseDTO> CreateEvent(EventCreateDTO eventModel);

        Task<EventResponseDTO> UpdateEvent(Guid id, EventDTO eventModel);

        Task<EventResponseDTO> DeleteEvent(Guid id);
        Task<EventResponseDTO> DeleteEventDatabase(Guid id);
    }
}