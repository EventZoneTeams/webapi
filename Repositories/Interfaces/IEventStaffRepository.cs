using EventZone.Domain.Entities;

namespace EventZone.Repositories.Interfaces
{
    public interface IEventStaffRepository : IGenericRepository<EventStaff>
    {
        Task<List<object>> GetUserListOrderAndTicket(Guid eventId);
    }
}
