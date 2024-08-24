namespace EventZone.Domain.Entities
{
    public class EventBoardTask : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid EventBoardColumnId { get; set; } // FK to EventBoardColumn
        public DateTime? DueDate { get; set; }
        public int? Priority { get; set; }
        public virtual EventBoardColumn EventBoardColumn { get; set; } // Navigation property
        public virtual ICollection<EventBoardTaskAssignment> EventBoardTaskAssignments { get; set; } // Navigation property to join table
        public virtual ICollection<EventBoardTaskLabelAssignment> EventBoardTaskLabelAssignments { get; set; } // Navigation property to join table
    }
}
