using System.Text.Json;
using NewUserManagement.Shared.Models;

public interface ILoggingServiceClient
{
    Task LogViewCount(int userId, int viewCount);
    Task LogEditCount(int userId, int editCount);
}

public class LoggingServiceClient : ILoggingServiceClient
{
    private readonly HttpClient _httpClient;

    public LoggingServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task LogViewCount(int userId, int viewCount)
    {
        var logEntry = new LogEntry
        {
            Timestamp = DateTime.UtcNow,
            UserId = userId,
            Action = "ViewCount",
            Details = "Viewed user profile.",
            ViewCount = viewCount
        };

        await LogAction(logEntry);
    }

    public async Task LogEditCount(int userId, int editCount)
    {
        var logEntry = new LogEntry
        {
            Timestamp = DateTime.UtcNow,
            UserId = userId,
            Action = "EditCount",
            Details = "Edited user profile.",
            EditCount = editCount
        };

        await LogAction(logEntry);
    }

    private async Task LogAction(LogEntry logEntry)
    {
        var json = JsonSerializer.Serialize(logEntry);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/LogEntries", content);
        response.EnsureSuccessStatusCode();
    }
}
