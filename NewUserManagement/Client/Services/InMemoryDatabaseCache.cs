using System.Net.Http.Json;
using NewUserManagement.Client.Static;
using NewUserManagement.Shared.Models;


namespace NewUserManagement.Client.Services
{
    internal sealed class InMemoryDatabaseCache
    {
        private const int PageSize = 10; // Number of users to fetch per page
        private readonly HttpClient _httpClient;

        public InMemoryDatabaseCache(HttpClient httpClient)
        {
            _httpClient = httpClient;
            OnUsersDataChanged += delegate { }; // Initializing with an empty delegate
        }

        private List<User>? _users = null;
        internal List<User> Users
        {
            get
            {
                return _users ?? new List<User>();
            }
            set
            {
                _users = value;
                NotifyUsersDataChanged();
            }
        }

        // Pagination properties
        internal int TotalPages { get; private set; }
        internal int CurrentPage { get; private set; }

        // Sorting properties
        internal string SortBy { get; private set; } = "Id"; // Default sorting by user ID
        internal bool AscendingOrder { get; private set; } = true; // Default ascending order

        private bool _gettingUsersFromDatabaseAndCaching = false;
        internal async Task GetUsersFromDatabaseAndCache(int? page)
        {
            if (_gettingUsersFromDatabaseAndCaching == false)
            {
                try
                {
                    _gettingUsersFromDatabaseAndCaching = true;
                    var url = $"{API_EndPoints.s_user}?pageSize={PageSize}";
                    if (page != null && page >= 1)
                    {
                        url += $"&page={page}";
                    }
                    var response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var responseData = await response.Content.ReadFromJsonAsync<List<User>>();
                    if (responseData != null)
                    {
                        Users = responseData;
                        CurrentPage = page ?? 1; // Set CurrentPage to page if not null, otherwise use 1
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Failed to fetch users from the server: {ex.Message}");
                    // Handle the error (e.g., display a message to the user)
                }
                finally
                {
                    _gettingUsersFromDatabaseAndCaching = false;
                }
            }
        }


        internal async Task<List<User>> GetActiveUsers(int page, int pageSize)
        {
            if (_users == null)
                await GetUsersFromDatabaseAndCache(page);

            var startIndex = (page - 1) * pageSize;
            return _users?.Where(u => u.IsActive).Skip(startIndex).Take(pageSize).ToList() ?? new List<User>();
        }

        internal async Task<List<User>> GetInactiveUsers(int page, int pageSize)
        {
            if (_users == null)
                await GetUsersFromDatabaseAndCache(page);

            var startIndex = (page - 1) * pageSize;
            return _users?.Where(u => !u.IsActive).Skip(startIndex).Take(pageSize).ToList() ?? new List<User>();
        }

        // Method to change sorting criteria
        internal async Task ChangeSorting(string sortBy, bool ascendingOrder)
        {
            SortBy = sortBy;
            AscendingOrder = ascendingOrder;
            // After changing sorting criteria, refetch users from the database
            await GetUsersFromDatabaseAndCache(CurrentPage);
        }

        // await ChangeSorting("Sorting Type Here", true); <--- use this when creating future methods to handle sorting.

        internal event Action? OnUsersDataChanged;

        private void NotifyUsersDataChanged() => OnUsersDataChanged?.Invoke();
    }

}
