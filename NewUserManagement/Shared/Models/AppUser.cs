using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NewUserManagement.Server.Data
{
    public class AppUser : IdentityUser
    {

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
