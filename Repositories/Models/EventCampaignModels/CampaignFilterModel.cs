namespace EventZone.Repositories.Models.EventCampaignModels
{
    public class CampaignFilterModel
    {
        public string SortBy { get; set; } = "id";
        public string SortDirection { get; set; } = "desc";
        public Guid? EventId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool? isDeleted { get; set; } = null;
    }
}