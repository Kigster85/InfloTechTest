namespace NewUserManagement.Shared.Models;
public class LogDBEntry
{
    public int LogId { get; set; }
    public DateTime Timestamp { get; set; }
    public string? UserId { get; set; }
    public string? Action { get; set; }
    public string? Details { get; set; }
    public int ViewCount { get; set; }
    public int EditCount { get; set; }
    public bool IsDeletedUserEntry { get; set; }
    public string? DeletedUserId { get; set; }
    public DateTime? DeletionTime { get; set; } // Nullable DateTime
}

