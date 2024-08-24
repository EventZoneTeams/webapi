namespace EventZone.Domain.DTOs.UserDTOs
{
    public class UserUpdateModel
    {
        public string? FullName { get; set; }
        public DateTime? Dob { get; set; }
        public string? Gender { get; set; }
        public string? ImageUrl { get; set; }
        public string? WorkAt { get; set; }
    }
}