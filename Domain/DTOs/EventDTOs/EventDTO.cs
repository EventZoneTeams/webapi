using Domain.Entities;
using Domain.Enums;

namespace Domain.DTOs.EventDTOs
{
    public class EventDTO : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }

        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public string Note { get; set; } = string.Empty;
        public string? Location { get; set; }
        public Guid UserId { get; set; }
        public Guid EventCategoryId { get; set; }
        public string? University { get; set; }
        public EventStatusEnums Status { get; set; } = EventStatusEnums.PENDING;
        public long? TotalCost { get; set; }
    }
}