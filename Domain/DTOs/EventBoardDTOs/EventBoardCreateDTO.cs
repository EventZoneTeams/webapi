namespace EventZone.Domain.DTOs.EventBoardDTOs
{
    public class EventBoardCreateDTO
    {
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Priority { get; set; }
        public string? Description { get; set; }
        public List<Guid>? EventBoardLabels { get; set; }
    }
}
