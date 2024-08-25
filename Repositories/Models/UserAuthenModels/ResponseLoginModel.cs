namespace EventZone.Repositories.Models.UserAuthenModels
{
    public class ResponseLoginModel
    {
        public string AccessToken { get; set; } = "";
        public string? RefreshToken { get; set; } = "";

        public DateTime? Expired { get; set; }

        public Guid? UserId { get; set; }
    }
}