

namespace NewUserManagement.Shared.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public required string Forename { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsSelected { get; set; }
    }
}
