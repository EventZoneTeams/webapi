namespace Domain.DTOs.EventDTOs
{
    public class EventDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? LocationDisplay { get; set; }
        public string? LocationNote { get; set; }
        public string? Note { get; set; }
        public Guid UserId { get; set; }
        public Guid EventCategoryId { get; set; }
        public string? Status { get; set; }
        public bool IsDeleted { get; set; }
    }
}