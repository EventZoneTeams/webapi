using EventZone.Domain.DTOs.BookedTicketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Services.Interface
{
    public interface IAttendeeService
    {
        Task<List<BookedTicketDetailDTO>> BookANewTicketForEvent(BookedTicketDTO bookedTicketDTO);
        Task<List<BookedTicketDetailDTO>> GetAllBookedTicket();
    }
}