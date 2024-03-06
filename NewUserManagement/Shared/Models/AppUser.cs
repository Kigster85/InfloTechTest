using Microsoft.AspNetCore.Identity;

namespace NewUserManagement.Shared.Models
{
    public class AppUser : IdentityUser
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }

    }
}
