using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewUserManagement.Shared.Models;
namespace NewUserManagement.Server.Data;

public class LogEntryConfiguration : IEntityTypeConfiguration<LogDBEntry>
{
    public void Configure(EntityTypeBuilder<LogDBEntry> builder)
    {
        builder.ToTable("LogEntries"); // Set table name
        builder.HasKey(e => e.LogId);      // Set primary key
        // Add additional configuration as needed
    }
}
