using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO.EventDonationDTOs
{
    public class EventDonationCreateDTO
    {
        [Required(ErrorMessage = "EventID is required!")]
        public int EventCampaignId { get; set; }

        public Int64 Amount { get; set; }
    }
}