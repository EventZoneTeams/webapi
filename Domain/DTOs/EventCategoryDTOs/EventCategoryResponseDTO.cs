namespace EventZone.Domain.DTOs.EventCategoryDTOs
{
    public class EventCategoryResponseDTO
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
