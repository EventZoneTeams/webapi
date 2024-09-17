using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EventZone.Domain.DTOs.UserDTOs
{
    public class UserSignupModel
    {
        [Required(ErrorMessage = "Email is required!"), EmailAddress(ErrorMessage = "Must be email format!")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password must be 5-20 Character")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Confirm Password is required!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation does not match!")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password must be 5-20 Character")]
        public string ConfirmPassword { get; set; } = "";

        [Required(ErrorMessage = "Full Name is required!")]
        public string? FullName { get; set; }

        public DateTime? Dob { get; set; }

        [Required(ErrorMessage = "Gender is required!")]
        public string? Gender { get; set; }

        public string? WorkAt { get; set; }

        public string? ImageUrl { get; set; }
    }
}