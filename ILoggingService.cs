using NewUserManagement.Server.Data;
using NewUserManagement.Shared.Models;


namespace NewUserManagement.Client.Services;

public interface ILoggingService
{
    Task LogAction(int userId, string action, string details);
}

public class LoggingService : ILoggingService
{
    private readonly AppDBContext _dbContext;

    public LoggingService(AppDBContext dbContext)
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

