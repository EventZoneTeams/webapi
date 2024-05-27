using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BusinessModels.UserModels
{
    public class UserUpdateModel
    {
        public string? FullName { get; set; }
        public DateTime? Dob { get; set; }
        public string? Gender { get; set; }
        public string? Image { get; set; }
        public string? University { get; set; }

    }
}
