
using System.ComponentModel.DataAnnotations;

namespace NewUserManagement.Shared.Models
{
    public class AppUserDTO
    {
        public string Id { get; set; }
        public string Password { get; set; }

        [Required(ErrorMessage = "Forename is required")]
        public string Forename { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public bool IsActive { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}
