using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NewUserManagement.Server.Data;


public class AppIdentityRoleConfig : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        // Configure your entity here
        // For example:
        builder.ToTable("IdentityRoles"); // Specify the table name
        builder.HasKey(role => role.Id); // Configure the primary key
                                         // Add additional configurations as needed    }
    }
    public enum UserRole
    {
        Admin,
        Member,
        Guest
    }

}


