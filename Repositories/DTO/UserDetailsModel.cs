namespace Repositories.DTO
{
    public class UserDetailsModel
    {
        public required int Id { get; set; }  // Sử dụng kiểu string cho Id
        public string Email { get; set; } = null!;


        public string? UnsignFullName { get; set; } = "";

        public string? FullName { get; set; }

        public DateTime? Dob { get; set; }

        public string? Gender { get; set; }

        public string? Image { get; set; } = "";
        public bool? IsDeleted { get; set; } = false;
        public List<RoleInfoModel>? Role { get; set; } = null;// 

    }
}
