using Domain.Entities;
using Domain.Enums;
using Repositories.Models;
using Services.DTO.EventCampaignDTOs;
using Services.DTO.EventCategoryDTOs;

namespace Services.DTO.EventDTOs
{
    public class EventResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public string Note { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int UserId { get; set; }
        public UserDetailsModel User { get; set; }
        public EventCategoryResponseDTO? EventCategory { get; set; }
        public string University { get; set; }
        public EventStatusEnums? Status { get; set; }
        public Int64 TotalCost { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual List<EventCampaignDTO>? EventCampaigns { get; set; }
    }
}