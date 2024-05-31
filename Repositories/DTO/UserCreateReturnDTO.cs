using Domain.Entities;

namespace Repositories.DTO
{
    public class UserCreateReturnDTO
    {
        public User User { get; set; }
        public string token { get; set; } = null!;
    }
}
