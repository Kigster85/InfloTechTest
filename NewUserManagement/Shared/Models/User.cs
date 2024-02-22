using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewUserManagement.Shared.Models;
public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Forename { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool IsActive { get; set; }

    // New DOB property
    public DateTime DateOfBirth { get; set; } 

    // Property to track whether the user is selected or not
    public bool IsSelected { get; set; }
}
