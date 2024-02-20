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
        internal async Task GetUsersFromDatabaseAndCache(int page)
        {
            if (_gettingUsersFromDatabaseAndCaching == false)
            {
                try
                {
                    _gettingUsersFromDatabaseAndCaching = true;
                    var response = await _httpClient.GetAsync($"{API_EndPoints.s_user}?page={page}&pageSize={PageSize}");
                    response.EnsureSuccessStatusCode();
                    var responseData = await response.Content.ReadFromJsonAsync<List<User>>();
                    if (responseData != null)
                    {
                        Users = responseData;
                        CurrentPage = page;
                        TotalPages = (int)Math.Ceiling((double)responseData.Count / PageSize);
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
