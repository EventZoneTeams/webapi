namespace Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; }
        public string? Body { get; set; }
        public string? Url { get; set; }
        public string? Sender { get; set; }
        public int? UserId { get; set; }
        public bool IsRead { get; set; } = false;
        public virtual User User { get; set; }
    }
}
