using Microsoft.AspNetCore.Identity;
using NewUserManagement.Shared.Models;

namespace NewUserManagement.Server.Data
{
    public static class AppDBSeedRoles
    {
        internal const string AdministratorRoleName = "Admin";
        internal const string AdministratorUserName = "admin@example.com";
        internal async static Task Seed(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            await SeedRoles(roleManager);
            await SeedAdminUser(userManager);
        }
        public async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in Enum.GetNames(typeof(UserRole)))
            {
                await SeedRole(roleManager, roleName);
            }
        }

        private async static Task SeedRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            bool roleExists = await roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                var role = new IdentityRole
                {
                    Name = roleName
                };

                await roleManager.CreateAsync(role);
            }
        }
        

        private async static Task SeedAdminUser(UserManager<AppUser> userManager)
        {
            bool administratorUserExists = await userManager.FindByEmailAsync(AdministratorUserName) != null;

            if (administratorUserExists == false)
            {
                var user = new AppUser
                {
                    UserName = AdministratorUserName,
                    Email = AdministratorUserName
                };
                // Make sure your Git repo is private if you do this
                IdentityResult identityResult = await userManager.CreateAsync(user, "AdminPassword123!");

                if (identityResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, AdministratorRoleName);
                }
            }
        }
    }
}
