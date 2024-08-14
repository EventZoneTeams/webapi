using EventZone.Domain.DTOs.EventDonationDTOs;
using EventZone.Domain.Enums;

namespace EventZone.Domain.DTOs.EventCampaignDTOs
{
    public class EventCampaignStaticticDTO
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EventCampaignStatusEnum Status { get; set; }
        public long CollectedAmount { get; set; }
        public long GoalAmount { get; set; }
        //các biến thống kế ở đây
        public int TotalDonors { get; set; } = 0;
        public decimal TargetAchievementPercentage { get; set; } = 0;
        public decimal AverageDonationAmount { get; set; } = 0;
        public long HighestDonationAmount { get; set; } = 0;
        public virtual ICollection<EventDonationDetailDTO>? EventDonations { get; set; }
    }
}