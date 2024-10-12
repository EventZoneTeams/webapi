using EventZone.Domain.Entities;

namespace EventZone.Services.Interface
{
    public interface IEventStaffService
    {
        Task<List<EventStaff>> GetEventStaffAsync(Guid eventId);
        Task<List<Event>> GetEventByCurrentStaff();
        Task<EventStaff> AddStaffIntoEvent(Guid eventId, Guid userId, string note);
        Task<EventStaff> RemoveStaffFromEvent(Guid eventId, Guid userId);
    }
}
