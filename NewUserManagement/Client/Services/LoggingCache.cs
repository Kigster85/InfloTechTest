using System.Net.Http.Json;
using static NewUserManagement.Client.Services.LoggingClientService;

namespace NewUserManagement.Client.Services
{
    public class LoggingCache
    {
        private List<LogEntry> _logEntries = new List<LogEntry>();
        // Property to expose log entries
        public List<LogEntry> LogEntries => _logEntries;

        // Method to clear log entries
        public void ClearLogEntries()
        {
            _logEntries.Clear();
        }
        public void AddLogEntry(LogEntry logEntry)
        {
            _logEntries.Add(logEntry);
        }

        public List<LogEntry> GetLogEntries()
        {
            return _logEntries;
        }

        // Additional methods for log retrieval, filtering, etc.
    }

    public class LoggingService
    {
        private readonly HttpClient _httpClient;
        private readonly LoggingCache _loggingCache;

        public LoggingService(HttpClient httpClient, LoggingCache loggingCache)
        {
            _httpClient = httpClient;
            _loggingCache = loggingCache;
        }

        public async Task FetchAndCacheLogEntries()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/logentries");
                response.EnsureSuccessStatusCode();

                var logEntries = await response.Content.ReadFromJsonAsync<List<LogEntry>>();
                if (logEntries != null)
                {
                    _loggingCache.ClearLogEntries(); // Clear existing cache
                    _loggingCache.LogEntries.AddRange(logEntries); // Add fetched log entries to cache
                }
            }
            catch (Exception ex)
            {
                // Handle error fetching log entries
                Console.WriteLine($"Error fetching log entries: {ex.Message}");
            }
        }

        // Method to retrieve cached log entries
        public List<LogEntry> GetCachedLogEntries()
        {
            return _loggingCache.LogEntries;
        }
    }
}
