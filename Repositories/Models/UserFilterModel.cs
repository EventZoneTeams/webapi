namespace EventZone.Repositories.Models
{
    public class UserFilterModel
    {

        public string SortBy { get; set; } = "id";
        public string SortDirection { get; set; } = "desc";
        public string? Role { get; set; }
        public bool? isDeleted { get; set; } = null;
        public string? Gender { get; set; }
        public string? SearchName { get; set; }

    }
}
