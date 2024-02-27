using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NewUserManagement.Server.Data;


public class AppUserConfig : IEntityTypeConfiguration<AppUser>
    {

        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers"); // Specify the table name



            // Configure other properties as needed
            builder.Property(u => u.Forename).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Surname).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
            builder.Property(u => u.EmailConfirmed).IsRequired().HasMaxLength(256);
            builder.Property(u => u.DateOfBirth).IsRequired().HasColumnType("date").HasDefaultValueSql("GETD");
            builder.Property(u => u.IsActive).HasColumnType("boolean");

    }
}

