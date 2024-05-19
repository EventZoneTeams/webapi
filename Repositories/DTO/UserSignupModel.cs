namespace Repositories.DTO
{
    public class UserSignupModel
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string? UnsignFullName { get; set; } = "";
        public string? FullName { get; set; }

        public DateTime? Dob { get; set; }

        public string? Gender { get; set; }
    }
}
