namespace Domain.Entities
{
    public class Event : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public string? ThumbnailUrl { get; set; }
        public DateTime? DonationStartDate { get; set; }
        public DateTime? DonationEndDate { get; set; }
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public string? Location { get; set; }
        public int UserId { get; set; }
        public int EventCategoryId { get; set; }
        public string? University { get; set; }
        public string? Status { get; set; }
        public string? OriganizationStatus { get; set; }
        public string? Note { get; set; }
        public bool IsDonation { get; set; }
        public decimal TotalCost { get; set; }

        public virtual User User { get; set; }
        public virtual EventCategory EventCategory { get; set; }
        public virtual ICollection<EventImage> EventImages { get; set; }
        public virtual ICollection<EventComment> EventComments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<EventOrder> EventOrders { get; set; }
        public virtual ICollection<EventPackage> EventPackages { get; set; }
        public virtual ICollection<EventProduct> EventProducts { get; set; }
        public virtual ICollection<EventFeedback> EventFeedbacks { get; set; }
    }
}
