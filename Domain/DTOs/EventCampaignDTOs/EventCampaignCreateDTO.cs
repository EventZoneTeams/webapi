using EventZone.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace EventZone.Domain.DTOs.EventCampaignDTOs
{
    public class EventCampaignCreateDTO
    {
        [Required(ErrorMessage = "Please input event for this campaign")]
        public Guid EventId { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EventCampaignStatusEnum Status { get; set; }
        public long GoalAmount { get; set; }
    }
}