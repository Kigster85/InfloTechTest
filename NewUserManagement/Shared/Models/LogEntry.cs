namespace NewUserManagement.Shared.Models;
public class LogEntry
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int UserId { get; set; }
    public string? Action { get; set; }
    public string? Details { get; set; }
    public int ViewCount { get; set; }
    public int EditCount { get; set; }

}

