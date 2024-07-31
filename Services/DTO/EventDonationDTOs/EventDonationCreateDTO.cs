using System.ComponentModel.DataAnnotations;

namespace Services.DTO.EventDonationDTOs
{
    public class EventDonationCreateDTO
    {
        [Required(ErrorMessage = "EventID is required!")]
        public Guid EventCampaignId { get; set; }

        public Int64 Amount { get; set; }
    }
}