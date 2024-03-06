using System.ComponentModel.DataAnnotations;

namespace NewUserManagement.Shared.Models;
public class AddUserDTO
{
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [CustomPasswordValidation(8, 20, requireDigit: true, requireLowercase: true, requireUppercase: true)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Forename is required")]
    public string Forename { get; set; }

    [Required(ErrorMessage = "Surname is required")]
    public string Surname { get; set; }

    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string EmailAddress { get; set; }
    public bool IsActive { get; set; }= true;

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
}
