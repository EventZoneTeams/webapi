using Domain.Entities;
using Domain.Enums;

namespace Services.DTO.EventDTOs
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
        public int UserId { get; set; }
        public int EventCategoryId { get; set; }
        public string? University { get; set; }
        public EventStatusEnums Status { get; set; } = EventStatusEnums.PENDING;
        public Int64? TotalCost { get; set; }
    }
}