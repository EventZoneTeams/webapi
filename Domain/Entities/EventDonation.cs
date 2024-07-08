namespace Domain.Entities
{
    public class EventDonation : BaseEntity
    {
        public int EventCampaignId { get; set; }
        public int UserId { get; set; }
        public Int64 Amount { get; set; }
        public DateTime DonationDate { get; set; }
        public virtual EventCampaign EventCampaign { get; set; }
        public virtual User User { get; set; }
    }
}
