namespace EventZone.Domain.Entities
{
    public class EventBoardTaskLabelAssignment
    {
        public Guid EventBoardTaskId { get; set; }
        public Guid EventBoardTaskLabelId { get; set; }
        public virtual EventBoardTask EventBoardTask { get; set; }
        public virtual EventBoardTaskLabel EventBoardTaskLabel { get; set; }
    }
}
