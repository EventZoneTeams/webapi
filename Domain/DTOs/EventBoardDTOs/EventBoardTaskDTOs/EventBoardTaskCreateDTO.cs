namespace EventZone.Domain.DTOs.EventBoardDTOs.EventBoardTaskDTOs
{
    public class EventBoardTaskCreateDTO
    {
        public Guid EventBoardColumnId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int? Priority { get; set; }
        public List<Guid>? EventBoardTaskLabelIds { get; set; }
    }
}
