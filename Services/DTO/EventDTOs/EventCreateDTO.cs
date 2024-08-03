using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Services.DTO.EventDTOs
{
    public class EventCreateDTO
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public IFormFile? ThumbnailUrl { get; set; }

        public DateTime? DonationStartDate { get; set; }

        public DateTime? DonationEndDate { get; set; }

        public DateTime? EventStartDate { get; set; }

        public DateTime? EventEndDate { get; set; }

        public string Note { get; set; }

        public string? Location { get; set; }

        public Guid UserId { get; set; }

        [Required]
        public Guid EventCategoryId { get; set; }

        public string? University { get; set; }
        public Int64? TotalCost { get; set; }
    }
}
