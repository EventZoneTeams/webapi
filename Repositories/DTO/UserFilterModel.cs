namespace Repositories.DTO
{
    public class UserFilterModel
    {

        public string Sort { get; set; } = "id";
        public string SortDirection { get; set; } = "desc";
        public string? Role { get; set; }
        public bool? isDeleted { get; set; }
        public string? Gender { get; set; }
        public string? Search { get; set; }

    }
}
