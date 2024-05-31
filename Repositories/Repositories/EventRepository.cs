using Domain.Entities;
using Repositories.Extensions;
using Repositories.Helper;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {

        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public EventRepository(StudentEventForumDbContext studentEventForumDbContext, ICurrentTime timeService, IClaimsService claims) : base(studentEventForumDbContext, timeService, claims)

        {
            _context = studentEventForumDbContext;
            _timeService = timeService;
            _claimsService = claims;
        }


        public IQueryable<Event> FilterAllField(EventParams eventParams)
        {
            var query = _context.Events
                .Search(eventParams.SearchTerm)
                .Filter(eventParams.EventCategoryId)
                .FilterByDonationDate(eventParams.DonationStartDate, eventParams.DonationEndDate)
                .FilterByEventDate(eventParams.EventStartDate, eventParams.EventEndDate)
                .FilterByLocation(eventParams.Location)
                .FilterByUniversity(eventParams.University)
                .FilterByStatus(eventParams.Status.ToString(), eventParams.OriganizationStatusEnums.ToString(), eventParams.IsDonation);

            return query;
        }

    }
}
