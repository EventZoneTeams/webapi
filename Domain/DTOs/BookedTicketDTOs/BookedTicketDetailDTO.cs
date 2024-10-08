using EventZone.Domain.DTOs.EventDTOs;
using EventZone.Domain.DTOs.TicketDTOs;
using EventZone.Domain.DTOs.UserDTOs;
using EventZone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Domain.DTOs.BookedTicketDTOs
{
    public class BookedTicketDetailDTO : BookedTicketDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public bool IsCheckedIn { get; set; }
        public Guid EventOrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual EventDTO? Event { get; set; }
      //  public virtual UserDTO? User { get; set; }
        public virtual EventTicketDTO? EventTicket { get; set; }
    }
}