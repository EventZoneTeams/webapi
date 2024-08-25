using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Repositories.Models.TicketModels
{
    public class BookedTicketFilterModel
    {
        public string SortBy { get; set; } = "id";
        public string SortDirection { get; set; } = "desc";
        public Guid? EventTicketId { get; set; }
        public Guid? EventId { get; set; }
        public Guid? UserId { get; set; }
        public bool? IsCheckedIn { get; set; } = false;
        public bool? IsDeleted { get; set; } = false;
    }
}