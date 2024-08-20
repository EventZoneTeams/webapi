namespace EventZone.Domain.Entities
{
    public class EventBoardTaskLabel : BaseEntity
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public Guid EventBoardId { get; set; } // FK to EventBoard
        public virtual EventBoard EventBoard { get; set; } // Navigation property
        public virtual ICollection<EventBoardTaskLabelAssignment> EventBoardTaskLabelAssignments { get; set; }
    }
}
