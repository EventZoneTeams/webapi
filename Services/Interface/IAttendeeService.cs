using EventZone.Domain.DTOs.BookedTicketDTOs;
using EventZone.Domain.DTOs.EventOrderDTOs;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Models.TicketModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Services.Interface
{
    public interface IAttendeeService
    {
        Task<List<BookedTicketDetailDTO>> BookANewTicketForEvent(BookedTicketRequestDTO bookedTicketDTO);

        Task<ApiResult<BookedTicketDetailDTO>> CheckinBookedAsync(Guid bookedId);

        Task<List<BookedTicketDetailDTO>> GetAllBookedTicketByOrderID(Guid orderId);

        Task<EventOrderBookedTicketDTO> GetAllBookedTicketOfCurrentUser();

        Task<List<BookedTicketDetailDTO>> GetAllBookedTickets();

        Task<Pagination<BookedTicketDetailDTO>> GetBookedsByFiltersAsync(PaginationParameter paginationParameter, BookedTicketFilterModel bookedTicketFilterModel);

        Task<EventOrderBookedTicketDTO> GetEventOrderWithTicket(Guid orderId);

        Task<ApiResult<BookedTicketDetailDTO>> UpdateBookedAsync(Guid bookedId, BookedTicketUpdateDTO updateModel);
    }
}