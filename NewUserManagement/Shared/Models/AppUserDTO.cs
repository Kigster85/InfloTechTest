using System.ComponentModel.DataAnnotations;

namespace NewUserManagement.Shared.Models
{
    public class AppUserDTO
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(80, ErrorMessage = "Your password must be between {2} and {1} characters.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string UserName { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public bool IsActive { get; set; }

        // New DOB property
        public DateTime DateOfBirth { get; set; }
    }
}
