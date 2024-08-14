namespace EventZone.Repositories.Models.ProductModels
{
    public class ProductFilterModel
    {
        public string SortBy { get; set; } = "id";
        public string SortDirection { get; set; } = "desc";
        public string? SearchName { get; set; }
        public Guid? EventId { get; set; }
        public long? MinPrice { get; set; }
        public long? MaxPrice { get; set; }

        public bool? isDeleted { get; set; } = null;
    }
}