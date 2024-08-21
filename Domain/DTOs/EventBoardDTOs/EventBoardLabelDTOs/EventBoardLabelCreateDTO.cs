namespace EventZone.Domain.DTOs.EventBoardDTOs.EventBoardLabelDTOs
{
    public class EventBoardLabelCreateDTO
    {
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
