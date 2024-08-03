using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.EventDonationDTOs
{
    public class EventDonationCreateDTO
    {
        [Required(ErrorMessage = "EventID is required!")]
        public Guid EventCampaignId { get; set; }

        public long Amount { get; set; }
    }
}