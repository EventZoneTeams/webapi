namespace Domain.Entities
{
    public class Event : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public string? Location { get; set; }
        public string? ReasonNote { get; set; }
        public Guid UserId { get; set; }
        public Guid EventCategoryId { get; set; }
        public string? University { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
        public virtual User User { get; set; }
        public virtual EventCategory EventCategory { get; set; }
        public virtual ICollection<EventImage> EventImages { get; set; }
        public virtual ICollection<EventComment> EventComments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<EventOrder> EventOrders { get; set; }
        public virtual ICollection<EventPackage> EventPackages { get; set; }
        public virtual ICollection<EventProduct> EventProducts { get; set; }
        public virtual ICollection<EventFeedback> EventFeedbacks { get; set; }
        public virtual ICollection<EventCampaign> EventCampaigns { get; set; }
    }
}
