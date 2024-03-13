using NewUserManagement.Shared.Models;

namespace NewUserManagement.Shared.Utilities;
public class UserMapper
{
    public static AppUserDTO Map(AppUser user)
    {
        AppUserDTO destination = new AppUserDTO
        {
            // Map properties manually
            Id = user.Id,
            // Map additional properties as needed
        };

        return destination;
    }
}

