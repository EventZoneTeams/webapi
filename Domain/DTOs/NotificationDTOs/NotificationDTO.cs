namespace EventZone.Domain.DTOs.NotificationDTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Url { get; set; }
        public string Sender { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}
