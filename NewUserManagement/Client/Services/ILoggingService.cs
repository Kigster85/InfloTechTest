using Microsoft.Extensions.Logging.Abstractions;
using NewUserManagement.Shared.Models;

namespace NewUserManagement.Client.Services;

public interface ILoggingService
{
    Task LogAction(int userId, string action, string details);
}

public class LoggingService : ILoggingService
{
    private readonly ApplicationDbContext _dbContext;

    public LoggingService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task LogAction(int userId, string action, string details)
    {
        var logEntry = new LogEntry
        {
            Timestamp = DateTime.UtcNow,
            UserId = userId,
            Action = action,
            Details = details
        };

        _dbContext.LogEntries.Add(logEntry);
        await _dbContext.SaveChangesAsync();
    }
}

