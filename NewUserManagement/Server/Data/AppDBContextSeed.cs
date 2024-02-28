using Microsoft.AspNetCore.Identity;

namespace NewUserManagement.Server.Data;

public class AppDBContextSeed
{
    public static void SeedUsers(UserManager<AppUser> userManager)
    {
        if (userManager.FindByEmailAsync("ploew@example.com").Result == null)
        {
            var user1 = new AppUser
            {
                UserName = "ploew@example.com",
                Email = "ploew@example.com",
                Forename = "Peter",
                Surname = "Loew",
                IsActive = true
            };

            var user2 = new AppUser
            {
                UserName = "bfgates@example.com",
                Email = "bfgates@example.com",
                Forename = "Benjamin Franklin",
                Surname = "Gates",
                IsActive = true
            };

            var user3 = new AppUser
            {
                UserName = "ctroy@example.com",
                Email = "ctroy@example.com",
                Forename = "Castor",
                Surname = "Troy",
                IsActive = false
            };

            var user4 = new AppUser
            {
                UserName = "mraines@example.com",
                Email = "mraines@example.com",
                Forename = "Memphis",
                Surname = "Raines",
                IsActive = true
            };

            var user5 = new AppUser
            {
                UserName = "sgodspeed@example.com",
                Email = "sgodspeed@example.com",
                Forename = "Stanley",
                Surname = "Goodspeed",
                IsActive = true
            };

            var user6 = new AppUser
            {
                UserName = "himcdunnough@example.com",
                Email = "himcdunnough@example.com",
                Forename = "H.I.",
                Surname = "McDunnough",
                IsActive = true
            };

            var user7 = new AppUser
            {
                UserName = "cpoe@example.com",
                Email = "cpoe@example.com",
                Forename = "Cameron",
                Surname = "Poe",
                IsActive = false
            };

            var user8 = new AppUser
            {
                UserName = "emalus@example.com",
                Email = "emalus@example.com",
                Forename = "Edward",
                Surname = "Malus",
                IsActive = false
            };

            var user9 = new AppUser
            {
                UserName = "dmacready@example.com",
                Email = "dmacready@example.com",
                Forename = "Damon",
                Surname = "Macready",
                IsActive = false
            };

            var user10 = new AppUser
            {
                UserName = "jblaze@example.com",
                Email = "jblaze@example.com",
                Forename = "Johnny",
                Surname = "Blaze",
                IsActive = true
            };

            var user11 = new AppUser
            {
                UserName = "rfeld@example.com",
                Email = "rfeld@example.com",
                Forename = "Robin",
                Surname = "Feld",
                IsActive = true
            };

            SeedUser(userManager, user1);
            SeedUser(userManager, user2);
            SeedUser(userManager, user3);
            SeedUser(userManager, user4);
            SeedUser(userManager, user5);
            SeedUser(userManager, user6);
            SeedUser(userManager, user7);
            SeedUser(userManager, user8);
            SeedUser(userManager, user9);
            SeedUser(userManager, user10);
            SeedUser(userManager, user11);
        }
    }

    private static void SeedUser(UserManager<AppUser> userManager, AppUser user)
    {
        var result = userManager.CreateAsync(user, "Password123!").Result;
        if (!result.Succeeded)
        {
            // Handle errors if user creation failed
        }
    }
}
