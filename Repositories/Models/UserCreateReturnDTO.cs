using EventZone.Domain.Entities;

namespace EventZone.Repositories.Models
{
    public class UserCreateReturnDTO
    {
        public User User { get; set; }
        public string token { get; set; } = null!;
    }
}
