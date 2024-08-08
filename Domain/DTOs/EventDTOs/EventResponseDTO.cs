using Domain.DTOs.EventCategoryDTOs;
using Domain.DTOs.UserDTOs;

namespace Domain.DTOs.EventDTOs
{
    public class EventResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public LocationResponseDTO? Location { get; set; }
        public string? Note { get; set; }
        public Guid UserId { get; set; }
        public Guid EventCategoryId { get; set; }
        public string? Status { get; set; }
        public bool IsDeleted { get; set; }
        public UserDetailsModel User { get; set; }
        public EventCategoryResponseDTO? EventCategory { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        //public List<EventPackageDetailDTO>? EventPackages { get; set; }
        //public virtual List<EventCampaignDTO>? EventCampaigns { get; set; }
    }
}