namespace Domain.Entities
{
    public class EventDonation : BaseEntity
    {
        public Guid EventCampaignId { get; set; }
        public Guid UserId { get; set; }
        public Int64 Amount { get; set; }
        public DateTime DonationDate { get; set; }
        public virtual EventCampaign EventCampaign { get; set; }
        public virtual User User { get; set; }
    }
}
