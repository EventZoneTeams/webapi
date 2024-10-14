namespace EventZone.Domain.Entities
{
    public class WithdrawnRequest : BaseEntity
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public string BankNote { get; set; }
        public long Amount { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
    }
}
