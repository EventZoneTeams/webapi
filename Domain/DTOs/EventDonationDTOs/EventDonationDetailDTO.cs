using EventZone.Domain.DTOs.UserDTOs;

namespace EventZone.Domain.DTOs.EventDonationDTOs
{
    public class EventDonationDetailDTO : EventDonationCreateDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public virtual UserDetailsModel? User { get; set; }
    }
}