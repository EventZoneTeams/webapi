using EventZone.Domain.DTOs.TicketDTOs;
using EventZone.Repositories.Commons;
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
        Task<ApiResult<EventTicketDetailDTO>> DeleteEventTicketByIdAsync(Guid id);
        Task<List<EventTicketDetailDTO>> GetAllTicketsByEventIdAsync(Guid eventId);
        Task<ApiResult<EventTicketDetailDTO>> GetTicketById(Guid id);
        Task<ApiResult<EventTicketDetailDTO>> UpdateEventTicketAsync(Guid ticketId, EventTicketUpdateDTO updateModel);
    }
}