using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class EventPackageDetailDTO
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int TotalPrice { get; set; }
        public string Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public bool? IsDeleted { get; set; } = false;

        public List<EventProductDetailDTO>? Products { get; set; }
    }
}