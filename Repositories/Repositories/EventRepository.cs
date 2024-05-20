using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly StudentEventForumDbContext _studentEventForumDbContext;

        public EventRepository(StudentEventForumDbContext studentEventForumDbContext)
        {
            _studentEventForumDbContext = studentEventForumDbContext;
        }

        public async Task<List<Event>> GetEventsAsync()
        {
            //get all events
            return await _studentEventForumDbContext.Events.ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            var eventEntity = await _studentEventForumDbContext.Events.FirstOrDefaultAsync(x => x.Id == id);
            //map event to eventModel
            //var eventModel = new EventModel
            //{
            //    Id = eventEntity.Id,
            //    Name = eventEntity.Name,
            //    Description = eventEntity.Description,
            //    DonationStartDate = eventEntity.DonationStartDate,
            //    DonationEndDate = eventEntity.DonationEndDate,
            //    EventStartDate = eventEntity.EventStartDate,
            //    EventEndDate = eventEntity.EventEndDate,
            //    Location = eventEntity.Location,
            //    UserId = eventEntity.UserId,
            //    University = eventEntity.University,
            //    Status = eventEntity.Status,
            //    DonationStatus = eventEntity.DonationStatus,
            //    IsDonation = eventEntity.IsDonation,
            //    TotalCost = eventEntity.TotalCost,
            //    CreatedAt = eventEntity.CreatedAt,
            //    CreatedBy = eventEntity.CreatedBy,
            //    ModifiedAt = eventEntity.ModifiedAt,
            //    ModifiedBy = eventEntity.ModifiedBy,
            //    DeletedAt = eventEntity.DeletedAt,
            //    DeletedBy = eventEntity.DeletedBy,
            //    IsDeleted = eventEntity.IsDeleted
            //};
            return eventEntity;
        }
    }
}
