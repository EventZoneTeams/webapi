using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class EventPackageDetailDTO
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int TotalPrice { get; set; }
        public string Description { get; set; }

        public List<EventProductDetailDTO>? Products { get; set; }


    }
}
