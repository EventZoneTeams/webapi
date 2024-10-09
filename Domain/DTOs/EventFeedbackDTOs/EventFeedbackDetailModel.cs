using EventZone.Domain.DTOs.UserDTOs;

namespace EventZone.Domain.DTOs.EventFeedbackDTOs
{
    public class EventFeedbackDetailModel
    {
        public Guid? EventId { get; set; }
        public string? Content { get; set; }
        public Guid? UserId { get; set; }

        public DateTime? CreatedAt { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}