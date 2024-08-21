namespace EventZone.Domain.Entities
{
    public class EventBoardLabelAssignment
    {
        public Guid EventBoardId { get; set; }
        public Guid EventBoardLabelId { get; set; }
        public virtual EventBoard EventBoard { get; set; }
        public virtual EventBoardLabel EventBoardLabel { get; set; }
    }
}
