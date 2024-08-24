namespace EventZone.Domain.Entities
{
    public class EventBoard : BaseEntity
    {
        public Guid EventId { get; set; } // FK to Event
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Priority { get; set; } // "Low", "Medium", "High"
        public string? Description { get; set; }
        public Guid? LeaderId { get; set; } // FK to Leader
        public virtual User Leader { get; set; }
        public virtual ICollection<EventBoardMember> EventBoardMembers { get; set; } // Navigation property to the join table
        public virtual Event Event { get; set; } // Navigation property
        public virtual ICollection<EventBoardColumn> EventBoardColumns { get; set; }
        public virtual ICollection<EventBoardTask> EventBoardTasks { get; set; }
        public virtual ICollection<EventBoardLabelAssignment> EventBoardLabelAssignments { get; set; } // Navigation property to join table
        public virtual ICollection<EventBoardTaskLabel> EventBoardTaskLabels { get; set; }
    }
}