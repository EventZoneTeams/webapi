using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Domain.DTOs.BookedTicketDTOs
{
    public class BookedTicketUpdateDTO
    {
        public bool? IsCheckedIn { get; set; }
        public string? AttendeeNote { get; set; }
    }
}