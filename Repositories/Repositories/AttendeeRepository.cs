using EventZone.Domain.Entities;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Interfaces;
using EventZone.Repositories.Models.EventCampaignModels;
using EventZone.Repositories.Models.TicketModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Repositories.Repositories
{
    public class AttendeeRepository : GenericRepository<BookedTicket>, IAttendeeRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public AttendeeRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public async Task<List<BookedTicket>> GetAllBookedTickets()
        {
            var result = await _context.BookedTickets.Include(x => x.Event).Include(x => x.EventTicket).ToListAsync();
            return result;
        }

        public async Task<List<BookedTicket>> GetAllBookedTicketsOfUser(Guid userid)
        {
            var result = await _context.BookedTickets.Include(x => x.Event).Include(x => x.EventTicket).Where(x => x.UserId == userid).ToListAsync();
            return result;
        }

        public async Task<List<BookedTicket>> GetBookedTicketsByOrderId(Guid orderId)
        {
            var result = await _context.BookedTickets.Include(x => x.Event).Include(x => x.EventTicket).Where(x => x.EventOrderId == orderId).ToListAsync();
            return result;
        }

        public async Task<EventOrder> GetOrderTicket(Guid orderId)
        {
            var result = await _context.EventOrders.FirstOrDefaultAsync(x => x.Id == orderId);
            return result;
        }

        public async Task<Pagination<BookedTicket>> GetBookedTicketsByFilterAsync(PaginationParameter paginationParameter, BookedTicketFilterModel bookedTicketFilter)
        {
            try
            {
                var BookedTicketsQuery = _context.BookedTickets.Include(x => x.Event).Include(x => x.EventTicket).AsNoTracking();
                BookedTicketsQuery = ApplyFilterSortAndSearch(BookedTicketsQuery, bookedTicketFilter);
                var sortedQuery = await ApplySorting(BookedTicketsQuery, bookedTicketFilter).ToListAsync();

                if (sortedQuery != null)
                {
                    var totalCount = sortedQuery.Count;
                    var bookedTicketssPagination = sortedQuery
                        .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                        .Take(paginationParameter.PageSize)
                        .ToList();
                    return new Pagination<BookedTicket>(bookedTicketssPagination, totalCount, paginationParameter.PageIndex, paginationParameter.PageSize);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private IQueryable<BookedTicket> ApplySorting(IQueryable<BookedTicket> query, BookedTicketFilterModel bookedTicketFilter)
        {
            try
            {
                switch (bookedTicketFilter.SortBy.ToLower())
                {
                    case "name":
                        query = bookedTicketFilter.SortDirection.ToLower() == "asc" ? query.OrderBy(a => a.EventTicket.Name) : query.OrderByDescending(a => a.EventTicket.Name);
                        break;

                    case "date":
                        query = bookedTicketFilter.SortDirection.ToLower() == "asc" ? query.OrderBy(a => a.CreatedAt) : query.OrderByDescending(a => a.CreatedAt);
                        break;

                    default:
                        query = bookedTicketFilter.SortDirection.ToLower() == "asc" ? query.OrderBy(a => a.Id) : query.OrderByDescending(a => a.Id);
                        break;
                }
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IQueryable<BookedTicket> ApplyFilterSortAndSearch(IQueryable<BookedTicket> query, BookedTicketFilterModel bookedTicketFilterModel)
        {
            if (bookedTicketFilterModel == null)
            {
                return query;
            }

            if (bookedTicketFilterModel.IsDeleted == true)
            {
                query = query.Where(a => a.IsDeleted == bookedTicketFilterModel.IsDeleted);
            }
            else if (bookedTicketFilterModel.IsDeleted == false)
            {
                query = query.Where(a => a.IsDeleted == bookedTicketFilterModel.IsDeleted);
            }

            if (bookedTicketFilterModel.IsCheckedIn == true)
            {
                query = query.Where(a => a.IsCheckedIn == bookedTicketFilterModel.IsCheckedIn);
            }
            else if (bookedTicketFilterModel.IsCheckedIn == false)
            {
                query = query.Where(a => a.IsCheckedIn == bookedTicketFilterModel.IsCheckedIn);
            }

            if (bookedTicketFilterModel.EventId.HasValue)
            {
                query = query.Where(p => p.EventId == bookedTicketFilterModel.EventId);
            }

            if (bookedTicketFilterModel.UserId.HasValue)
            {
                query = query.Where(p => p.UserId == bookedTicketFilterModel.UserId);
            }

            if (bookedTicketFilterModel.EventTicketId.HasValue)
            {
                query = query.Where(p => p.EventTicketId == bookedTicketFilterModel.EventTicketId);
            }

            return query;
        }
    }
}