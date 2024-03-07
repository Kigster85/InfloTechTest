using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NewUserManagement.Shared.Models
{
    public class AppUser : IdentityUser
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string emailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [CustomPasswordValidation(8, 20, requireDigit: true, requireLowercase: true, requireUppercase: true)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Forename is required")]
        [Display(Name = "Forename")]
        public string Forename { get; set; } = default!;

        [Required(ErrorMessage = "Surname is required")]
        [Display(Name = "Surname")]
        public string Surname { get; set; } = default!;
        public bool IsActive { get; set; }

        // New DOB property
        [Required(ErrorMessage = "Date Of Birth is required")]
        public DateTime DateOfBirth { get; set; }
    }
}
