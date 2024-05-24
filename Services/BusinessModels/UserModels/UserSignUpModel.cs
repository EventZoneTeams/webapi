namespace Services.BusinessModels.UserModels
{
    public class UserSignUpModel
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";

        public string? UnsignFullName { get; set; } = "";
        public string? FullName { get; set; }

        public DateTime? Dob { get; set; }

        public bool? Gender { get; set; }
    }
}
