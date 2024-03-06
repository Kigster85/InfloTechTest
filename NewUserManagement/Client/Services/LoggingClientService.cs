using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace NewUserManagement.Client.Services
{
    public class LoggingClientService
    {
        public class LogEntry
        {
            public string LogId { get; set; }
            public DateTime Timestamp { get; set; }
            public string? UserId { get; set; }
            public string? Action { get; set; }
            public string? Details { get; set; }
            public int ViewCount { get; set; }
            public int EditCount { get; set; }
            public bool IsDeletedUserEntry { get; set; }
            public string? DeletedUserId { get; set; }
            public DateTime? DeletionTime { get; set; } // Nullable DateTime

            // Constructor for regular log entries
            public LogEntry(string logId, string userId, string action, DateTime timestamp)
            {
                LogId = logId;
                UserId = userId;
                Action = action;
                Timestamp = timestamp;
            }

            // Constructor for deleted user entries
            public LogEntry(string logId, string deletedUserId, DateTime deletionTime)
            {
                LogId = logId;
                DeletedUserId = deletedUserId;
                DeletionTime = deletionTime;
                IsDeletedUserEntry = true;
            }
            public LogEntry()
            {
                // Parameterless constructor
            }
        }

        public interface ILogService
        {
            Task<List<LogEntry>> GetLogEntriesAsync();
            Task AddLogEntryAsync(LogEntry logEntry);
            Task LogAsync(LogEntry logEntry);
        }

        public class LogService : ILogService
        {
            private readonly ILogger<LogService> _logger;
            private readonly HttpClient _httpClient;

            public LogService(ILogger<LogService> logger, HttpClient httpClient)
            {
                _logger = logger;
                _httpClient = httpClient;
            }

            public async Task<List<LogEntry>> GetLogEntriesAsync()
            {
                try
                {
                    // Fetch log entries from the server
                    var response = await _httpClient.GetAsync("api/logentries");
                    response.EnsureSuccessStatusCode();

                    // Read the response content as JSON
                    var content = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON content to a list of LogEntry objects
                    var logEntries = JsonSerializer.Deserialize<List<LogEntry>>(content);

                    if (logEntries != null)
                    {
                        // Log the retrieval of log entries
                        _logger.LogInformation("Retrieved log entries: Count = {LogEntryCount}", logEntries.Count);

                        return logEntries;
                    }
                    else
                    {
                        // Log a warning if the log entries are null
                        _logger.LogWarning("Failed to retrieve log entries: Response content is null.");
                        return new List<LogEntry>(); // Return an empty list
                    }
                }
                catch (Exception ex)
                {
                    // Log any exceptions that occur during log retrieval
                    _logger.LogError(ex, "Error occurred while retrieving log entries: {ErrorMessage}", ex.Message);
                    throw; // Re-throw the exception to propagate it to the caller
                }
            }

            public async Task AddLogEntryAsync(LogEntry logEntry)
            {
                try
                {
                    // Send a POST request to the server to add the log entry
                    var response = await _httpClient.PostAsJsonAsync("api/logentries", logEntry);
                    response.EnsureSuccessStatusCode();

                    // Log a message indicating that the log entry was successfully added
                    _logger.LogInformation("Log entry added successfully.");
                }
                catch (Exception ex)
                {
                    // Log or handle any exceptions that occur during log entry addition
                    _logger.LogError(ex, "Error occurred while adding log entry: {ErrorMessage}", ex.Message);
                    throw; // Re-throw the exception to propagate it to the caller
                }
            }
            public async Task LogAsync(LogEntry logEntry)
            {
                try
                {
                    // Serialize the log entry object to JSON
                    string logEntryJson = JsonSerializer.Serialize(logEntry);

                    // Prepare the HTTP content with the serialized log entry
                    var content = new StringContent(logEntryJson, Encoding.UTF8, "application/json");

                    // Send a POST request to the server to log the entry
                    var response = await _httpClient.PostAsync("api/logentries", content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Log a message indicating successful logging
                        _logger.LogInformation("Log entry added successfully: {@LogEntry}", logEntry);
                    }
                    else
                    {
                        // Log a warning if the logging request failed
                        _logger.LogWarning("Failed to add log entry. Status code: {StatusCode}", response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    // Log any exceptions that occur during logging
                    _logger.LogError(ex, "Error occurred while adding log entry: {ErrorMessage}", ex.Message);
                    throw; // Re-throw the exception to propagate it to the caller
                }
            }



        }
    }
}
