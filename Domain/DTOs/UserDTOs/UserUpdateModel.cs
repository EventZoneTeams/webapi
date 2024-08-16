namespace EventZone.Domain.DTOs.UserDTOs
{
    public class UserUpdateModel
    {
        public string? FullName { get; set; }
        public DateTime? Dob { get; set; }
        public string? Gender { get; set; }
        public string? Image { get; set; }
        public string? University { get; set; }

    }
}
