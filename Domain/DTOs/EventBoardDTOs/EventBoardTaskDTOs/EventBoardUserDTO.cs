namespace EventZone.Domain.DTOs.EventBoardDTOs.EventBoardTaskDTOs
{
    public class EventBoardUserDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string? UnsignFullName { get; set; } = "";
        public string? FullName { get; set; }
        public DateTime? Dob { get; set; }
        public string? Gender { get; set; }
        public string? Image { get; set; } = "";
        public string? University { get; set; }
    }
}
