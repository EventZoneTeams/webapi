using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardColumnDTOs;
using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardTaskLabelDTOs;

namespace EventZone.Domain.DTOs.EventBoardDTOs.EventBoardTaskDTOs
{
    public class EventBoardTaskResponseDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int? Priority { get; set; }
        public EventBoardColumnDTO EventBoardColumn { get; set; } // Navigation property
        public List<EventBoardTaskAssignmentDTO> EventBoardTaskAssignments { get; set; }
        public List<EventBoardTaskLabelDTO> EventBoardTaskLabels { get; set; } // Navigation property to join table
    }
}
