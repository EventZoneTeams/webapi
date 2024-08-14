namespace EventZone.Repositories.Models.PackageModels
{
    public class PackageFilterModel
    {
        public string SortBy { get; set; } = "id";
        public string SortDirection { get; set; } = "desc";
        public Guid? EventId { get; set; }
        public long? MinTotalPrice { get; set; }
        public long? MaxTotalPrice { get; set; }
        public bool? isDeleted { get; set; } = null;
    }
}