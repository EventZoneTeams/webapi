namespace Repositories.Models.PackageModels
{
    public class PackageFilterModel
    {
        public string SortBy { get; set; } = "id";
        public string SortDirection { get; set; } = "desc";
        public Guid? EventId { get; set; }
        public Int64? MinTotalPrice { get; set; }
        public Int64? MaxTotalPrice { get; set; }
        public bool? isDeleted { get; set; } = null;
    }
}