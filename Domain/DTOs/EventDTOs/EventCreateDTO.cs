namespace Domain.DTOs.EventDTOs
{
    public class EventCreateDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public LocationResponseDTO? Location { get; set; }
        public string? Note { get; set; }
        public Guid EventCategoryId { get; set; }
    }
}
