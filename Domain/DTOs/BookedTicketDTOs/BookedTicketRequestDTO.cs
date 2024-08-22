using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Domain.DTOs.BookedTicketDTOs
{
    public class BookedTicketRequestDTO
    {
        public Guid EventTicketId { get; set; }
        public Guid EventId { get; set; }

        public string AttendeeNote { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}