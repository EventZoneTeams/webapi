namespace EventZone.Domain.DTOs.EventBoardDTOs.EventBoardTaskLabelDTOs
{
    public class EventBoardTaskLabelCreateDTO
    {
        public Guid EventBoardId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
