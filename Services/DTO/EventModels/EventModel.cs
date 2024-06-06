using Domain.Entities;
using Domain.Enums;

namespace Services.DTO.EventModels
{
    public class EventModel : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public DateTime? DonationStartDate { get; set; }
        public DateTime? DonationEndDate { get; set; }
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public string? Note { get; set; }
        public string? Location { get; set; }
        public int UserId { get; set; }
        public int EventCategoryId { get; set; }
        public string? University { get; set; }
        public EventStatusEnums Status { get; set; } = EventStatusEnums.PENDING;
        public OriganizationStatusEnums OriganizationStatus { get; set; } = OriganizationStatusEnums.PREPARING;
        public bool? IsDonation { get; set; }
        public decimal? TotalCost { get; set; }
    }
}
