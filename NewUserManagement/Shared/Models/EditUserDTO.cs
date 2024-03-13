using System.ComponentModel.DataAnnotations;

namespace NewUserManagement.Shared.Models;
public class EditUserDTO
{
    public string Id { get; set; }  

    public string Forename { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
    public bool IsActive { get; set; }
}
