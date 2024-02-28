using System.ComponentModel.DataAnnotations;

namespace NewUserManagement.Shared.Models;
public class User
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Forename is required")]
    [Display(Name = "Forename")]
    public string Forename { get; set; } = default!;

    [Required(ErrorMessage = "Surname is required")]
    [Display(Name = "Surname")]
    public string Surname { get; set; } = default!;

    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Display(Name = "Email address")]
    public string Email { get; set; } = default!;
    public bool IsActive { get; set; }

    // New DOB property
    [Required(ErrorMessage = "Date Of Birth is required")]
    public DateTime DateOfBirth { get; set; } 


}
