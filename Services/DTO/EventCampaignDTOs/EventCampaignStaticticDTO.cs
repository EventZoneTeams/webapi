using Domain.Entities;
using Domain.Enums;
using Services.DTO.EventDonationDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO.EventCampaignDTOs
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
        public Int64 CollectedAmount { get; set; }

        public Int64 GoalAmount { get; set; }

        //các biến thống kế ở đây
        public int TotalDonors { get; set; } = 0;

        public decimal TargetAchievementPercentage { get; set; } = 0;
        public decimal AverageDonationAmount { get; set; } = 0;
        public Int64 HighestDonationAmount { get; set; } = 0;

        public virtual ICollection<EventDonationDetailDTO>? EventDonations { get; set; }
    }
}