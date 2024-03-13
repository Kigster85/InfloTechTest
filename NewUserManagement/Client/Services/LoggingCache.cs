using static NewUserManagement.Client.Services.LoggingClientService;
using System.Net.Http.Json;

namespace NewUserManagement.Client.Services
{
    public class LoggingCache
    {
        private List<LogEntry> _logEntries = new List<LogEntry>();

        // Property to expose log entries
        public List<LogEntry> LogEntries => _logEntries;

        // Method to add log entries to the cache
        public void AddLogEntries(List<LogEntry> logEntries)
        {
            _logEntries.AddRange(logEntries);
        }

        // Method to clear log entries
        public void ClearLogEntries()
        {
            _logEntries.Clear();
        }

        // Asynchronous method to fetch log entries from the cache
        public Task<List<LogEntry>> GetCachedLogEntriesAsync()
        {
            return Task.FromResult(_logEntries);
        }
        public Task<LogEntry> GetLogEntryDetailsAsync(int logId)
        {
            return Task.FromResult(_logEntries.Find(entry => entry.LogId == logId));
        }
    }

    public class LoggingService
    {
        private readonly HttpClient _httpClient;
        private readonly LoggingCache _loggingCache;
        private readonly TimeSpan _cacheRefreshInterval = TimeSpan.FromMinutes(30); // Cache refresh interval (e.g., every 30 minutes)
        private DateTime _lastCacheRefreshTime;
        private readonly System.Timers.Timer _refreshTimer;
        public LoggingService(HttpClient httpClient, LoggingCache loggingCache)
        {
            _httpClient = httpClient;
            _loggingCache = loggingCache;

            // Initialize the timer for cache refresh
            _refreshTimer = new System.Timers.Timer(_cacheRefreshInterval.TotalMilliseconds);
            _refreshTimer.Elapsed += async (sender, e) => await RefreshCache();
            _refreshTimer.AutoReset = true;
            _refreshTimer.Start();

            _lastCacheRefreshTime = DateTime.UtcNow;
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
                    _loggingCache.AddLogEntries(logEntries); // Add fetched log entries to cache
                    _lastCacheRefreshTime = DateTime.UtcNow; // Update last cache refresh time
                }
            }
            catch (Exception ex)
            {
                // Handle error fetching log entries
                Console.WriteLine($"Error fetching log entries: {ex.Message}");
            }
        }

        // Method to refresh the cache
        private async Task RefreshCache()
        {
            // Refresh only if the interval has passed since the last refresh
            if (DateTime.UtcNow - _lastCacheRefreshTime >= _cacheRefreshInterval)
            {
                await FetchAndCacheLogEntries();
            }
        }

        public Task<LogEntry> GetLogEntryDetailsAsync(int logId)
        {
            return _loggingCache.GetLogEntryDetailsAsync(logId);
        }
    }
}
