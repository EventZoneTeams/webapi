namespace Services.BusinessModels.EmailModels
{
    public class EmailConfiguration
    {
        public string From { get; set; } = null!;
        public string SmtpServer { get; set; } = null!;
        public int Port { get; set; }
        public String Username { get; set; } = null!;
        public String Password { get; set; } = null!;
    }
}
