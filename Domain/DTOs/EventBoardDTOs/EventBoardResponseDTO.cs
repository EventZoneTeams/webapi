using EventZone.Domain.Entities;

namespace EventZone.Domain.DTOs.EventBoardDTOs
{
    public class EventBoardResponseDTO
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; } // FK to Event
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Priority { get; set; } // "Low", "Medium", "High"
        public string? Description { get; set; }
        public Guid? LeaderId { get; set; }
        public User? Leader { get; set; }
        public ICollection<EventBoardMember> EventBoardMembers { get; set; }
        public Event Event { get; set; }
        public ICollection<EventBoardColumn> EventBoardColumns { get; set; }
        public ICollection<EventBoardTask> EventBoardTasks { get; set; }
        public ICollection<EventBoardLabelAssignment> EventBoardLabelAssignments { get; set; }
    }
}
