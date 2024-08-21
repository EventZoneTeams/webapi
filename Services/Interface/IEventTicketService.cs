using EventZone.Domain.DTOs.TicketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Services.Interface
{
    public interface IEventTicketService
    {
        Task<EventTicketDetailDTO> CreateNewTicketAsync(EventTicketDTO createTicket);

        Task<List<EventTicketDetailDTO>> GetAllTicketsByEventIdAsync(Guid eventId);
    }
}