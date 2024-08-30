namespace EventZone.Domain.DTOs.UserDTOs
{
    public class UserDetailsModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;

        public string? UnsignFullName { get; set; } = "";

        public string? FullName { get; set; }

        public DateTime? Dob { get; set; }

        public string? Gender { get; set; }

        public string? WorkAt { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public string? ImageUrl { get; set; } = "";

        //public string? RoleName { get; set; } = null;
        public RoleInfoModel? Role { get; set; } = null;
    }
}