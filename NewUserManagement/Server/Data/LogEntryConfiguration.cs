using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewUserManagement.Shared.Models;

public class LogEntryConfiguration : IEntityTypeConfiguration<LogEntry>
{
    public void Configure(EntityTypeBuilder<LogEntry> builder)
    {
        builder.ToTable("LogEntries"); // Set table name
        builder.HasKey(e => e.Id);      // Set primary key
        // Add additional configuration as needed
    }
}
