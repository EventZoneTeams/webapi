using EventZone.Domain.Entities;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Models.TicketModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Repositories.Interfaces
{
    public interface IAttendeeRepository : IGenericRepository<BookedTicket>
    {
        Task<List<BookedTicket>> GetAllBookedTickets();

        Task<List<BookedTicket>> GetBookedTicketsByOrderId(Guid orderId);

        Task<Pagination<BookedTicket>> GetBookedTicketsByFilterAsync(PaginationParameter paginationParameter, BookedTicketFilterModel bookedTicketFilter);

        Task<EventOrder> GetOrderTicket(Guid orderId);

        Task<List<BookedTicket>> GetAllBookedTicketsOfUser(Guid userId);
    }
}