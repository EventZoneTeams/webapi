namespace EventZone.Domain.Entities
{
    public class EventBoardLabel : BaseEntity
    {
        public Guid EventId { get; set; } // FK to Event
        public string Name { get; set; }
        public string Color { get; set; }
        public virtual Event Event { get; set; } // Navigation property
        public virtual ICollection<EventBoardLabelAssignment> EventBoardLabelAssignments { get; set; } // Navigation property to join table
    }
}
