namespace EventZone.Domain.Entities
{
    public class EventBoardColumn : BaseEntity
    {
        public Guid EventBoardId { get; set; } // FK to EventBoard
        public string Name { get; set; }
        public string Color { get; set; }
        public virtual EventBoard EventBoard { get; set; } // Navigation property
        public virtual ICollection<EventBoardTask> EventBoardTasks { get; set; }
    }
}
