namespace EventZone.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; }
        public string? Body { get; set; }
        public string? Url { get; set; }
        public Guid? ReceiverId { get; set; }
        public string Type { get; set; } // USER, ROLE, ALL
        public bool IsRead { get; set; } = false;
    }
}
