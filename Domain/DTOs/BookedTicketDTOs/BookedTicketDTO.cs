using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Domain.DTOs.BookedTicketDTOs
{
    public class BookedTicketDTO
    {
        public Guid EventTicketId { get; set; }
        public Guid EventId { get; set; }

        //public Guid UserId { get; set; }
        //public Int64 PaidPrice { get; set; }

        public string AttendeeNote { get; set; }
        public int? Quantity { get; set; }
    }
}