using System.ComponentModel.DataAnnotations;

namespace EventZone.Domain.DTOs.EventDonationDTOs
{
    public class EventDonationCreateDTO
    {
        [Required(ErrorMessage = "EventID is required!")]
        public Guid EventCampaignId { get; set; }

        public long Amount { get; set; }
    }
}