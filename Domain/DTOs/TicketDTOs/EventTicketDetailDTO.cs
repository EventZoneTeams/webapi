using EventZone.Domain.DTOs.EventDTOs;
using EventZone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Domain.DTOs.TicketDTOs
{
    public class EventTicketDetailDTO : EventTicketDTO
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual EventDTO? Event { get; set; }
    }
}