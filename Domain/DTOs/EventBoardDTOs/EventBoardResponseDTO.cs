using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardColumnDTOs;
using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardLabelDTOs;
using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardTaskLabelDTOs;
using EventZone.Domain.DTOs.EventDTOs;
using EventZone.Domain.DTOs.UserDTOs;

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
        public UserDetailsModel? Leader { get; set; }
        public EventDTO Event { get; set; }
        public ICollection<EventBoardColumnDTO> EventBoardColumns { get; set; }
        public ICollection<EventBoardLabelAssignmentDTO> EventBoardLabels { get; set; }
        public ICollection<EventBoardTaskLabelDTO> EventBoardTaskLabels { get; set; }
    }
}
