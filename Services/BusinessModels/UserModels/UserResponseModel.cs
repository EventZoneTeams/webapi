namespace Services.BusinessModels.UserModels
{
    public class UserResponseModel
    {
        public bool Status { get; set; } = false;

        public string Message { get; set; } = "";

        public string JWT { get; set; } = "";

        public DateTime? Expired { get; set; }

        public string? JWTRefreshToken { get; set; } = "";
    }
}
