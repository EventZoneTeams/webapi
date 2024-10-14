using EventZone.Domain.DTOs.EventDTOs;
using EventZone.Domain.DTOs.UserDTOs;
using EventZone.Domain.Entities;

namespace EventZone.Services.Interface
{
    public interface IEventStaffService
    {
        Task<List<UserDetailsModel>> GetEventStaffAsync(Guid eventId);
        Task<List<EventResponseDTO>> GetEventByCurrentStaff();
        Task<EventStaff> AddStaffIntoEvent(Guid eventId, Guid userId, string note);
        Task<EventStaff> RemoveStaffFromEvent(Guid eventId, Guid userId);
        Task<object> GetUserListOrderAndTicket(Guid eventId);
    }
}
