using Domain.DTOs.UserDTOs;
using Repositories.Models;

namespace Services.DTO.EventFeedbackModel
{
    public class EventFeedbackDetailModel
    {
        public Guid? EventId { get; set; }
        public string? Content { get; set; }
        // public int? UserId { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool? IsDeleted { get; set; } = false;

        // public virtual EventResponseDTO? Event { get; set; } = null;

        public virtual UserDetailsModel? User { get; set; }
        //  public string? FeedbackType { get; set; }
    }
}