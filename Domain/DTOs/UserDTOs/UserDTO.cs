using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Domain.DTOs.UserDTOs
{
    public class UserDTO
    {
        public string Email { get; set; } = null!;

        public string? UnsignFullName { get; set; } = "";

        public string? FullName { get; set; }

        public DateTime? Dob { get; set; }

        public string? Gender { get; set; }

        public string? ImageUrl { get; set; } = "";
        public string? WorkAt { get; set; }
    }
}