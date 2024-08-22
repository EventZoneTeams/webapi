using EventZone.Domain.DTOs.BookedTicketDTOs;
using EventZone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Domain.DTOs.EventOrderDTOs
{
    public class EventOrderBookedTicketDTO
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public string OrderType { get; set; } //PRODUCT OR TICKET
        public long TotalAmount { get; set; }
        public string Status { get; set; }
        public virtual ICollection<BookedTicketDTO>? BookedTickets { get; set; }
    }
}