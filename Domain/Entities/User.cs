using Microsoft.AspNetCore.Identity;

namespace EventZone.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string? FullName { get; set; }
        public string? UnsignFullName { get; set; } = "";
        public DateTime? Dob { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? Gender { get; set; }
        public string? ImageUrl { get; set; } = "";
        public string? WorkAt { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? DeletionDate { get; set; }
        public Guid? DeleteBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public virtual ICollection<Wallet> Wallets { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<EventBoardMember> EventBoardMembers { get; set; }
        public virtual ICollection<EventBoardTaskAssignment> EventBoardTaskAssignments { get; set; } // Navigation property to join table
        public virtual ICollection<BookedTicket>? BookedTickets { get; set; } // Navigation property to join table
    }
}