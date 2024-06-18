using Domain.Enums;
using Repositories.Models;
using Services.DTO.EventCategoryDTOs;

namespace Services.DTO.EventDTOs
{
    public class EventResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime DonationStartDate { get; set; }
        public DateTime DonationEndDate { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public string Note { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int UserId { get; set; }
        public UserDetailsModel User { get; set; }
        public EventCategoryResponseDTO? EventCategory { get; set; }
        public string University { get; set; }
        public EventStatusEnums? Status { get; set; } = EventStatusEnums.PENDING;
        public OriganizationStatusEnums? OriganizationStatus { get; set; } = OriganizationStatusEnums.PREPARING;
        public bool IsDonation { get; set; }
        public decimal TotalCost { get; set; }
    }
}
