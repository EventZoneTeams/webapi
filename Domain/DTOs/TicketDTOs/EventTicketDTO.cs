using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Domain.DTOs.TicketDTOs
{
    public class EventTicketDTO
    {
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid EventId { get; set; }
        public int InStock { get; set; }
        public Int64 Price { get; set; }
    }
}