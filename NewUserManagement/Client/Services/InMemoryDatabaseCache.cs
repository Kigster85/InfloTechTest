using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using NewUserManagement.Shared.Models;

namespace NewUserManagement.Client.Services
{
    public class InMemoryDatabaseCache
    {
        private readonly HttpClient _httpClient;

        public InMemoryDatabaseCache(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private List<AppUser>? _users = null;
        internal List<AppUser> Users
        {
            get
            {
                return _users ?? new List<AppUser>();
            }
            set
            {
                _users = value;
                NotifyUsersDataChanged();
            }
        }
        public List<string> SelectedUserIds { get; private set; } = new List<string>();

        // Pagination properties
        internal int PageSize { get; set; } = 25; // Number of users to fetch per page
        internal int TotalPages { get; private set; }
        internal int CurrentPage { get; private set; }
        // Sorting properties
        internal string SortBy { get; private set; } = "Id"; // Default sorting by user ID
        internal bool AscendingOrder { get; private set; } = true; // Default ascending order
        internal event Action? OnUsersDataChanged;

        private bool _gettingUsersFromDatabaseAndCaching = false;
        private static readonly object lockObject = new object();

        private void NotifyUsersDataChanged() => OnUsersDataChanged?.Invoke();

        // Dictionary to store view counts for users
        private Dictionary<string, int> _userViewCounts = new Dictionary<string, int>();

        // Assuming User is your custom user model and AppUser is the ASP.NET Core Identity user model
        internal async Task GetUsersFromDatabaseAndCache()
        {
            if (_gettingUsersFromDatabaseAndCaching)
                return;

            try
            {
                _gettingUsersFromDatabaseAndCaching = true;

                // Make a call to your API to fetch all users
                var response = await _httpClient.GetAsync("api/user");

                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadFromJsonAsync<List<AppUser>>();

                    Users = users!;
                    Console.WriteLine("Data retrieved from the API and cached successfully.");
                }
                else
                {
                    // Handle unsuccessful response (e.g., log error, display error message)
                    Console.WriteLine($"Failed to fetch users from the API: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch users from the API: {ex.Message}");
                // Handle the error (e.g., log error, display error message)
            }
            finally
            {
                _gettingUsersFromDatabaseAndCaching = false;
            }
        }
        public async Task<List<AppUser>> GetAllUsers()
        {
            await GetUsersFromDatabaseAndCache();
            return Users;
        }

        internal async Task<AppUser?> GetUserDetails(string? userId)
        {
            if (userId == null)
                return null;

            try
            {
                // Make a call to your API to fetch user details by ID
                var response = await _httpClient.GetAsync($"api/user/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<AppUser>();
                    return user;
                }
                else
                {
                    // Handle unsuccessful response (e.g., log error, display error message)
                    Console.WriteLine($"Failed to fetch user details from the API: {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch user details from the API: {ex.Message}");
                // Handle the error (e.g., log error, display error message)
                return null;
            }
        }


        internal async Task<List<AppUser>> GetActiveUsers(int page)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/user/active?page={page}");

                if (response.IsSuccessStatusCode)
                {
                    var activeUsers = await response.Content.ReadFromJsonAsync<List<AppUser>>();
                    return activeUsers!;
                }
                else
                {
                    Console.WriteLine($"Failed to fetch active users from the API: {response.ReasonPhrase}");
                    return new List<AppUser>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch active users from the API: {ex.Message}");
                return new List<AppUser>();
            }
        }

        internal async Task<List<AppUser>> GetInactiveUsers(int page)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/user/inactive?page={page}");

                if (response.IsSuccessStatusCode)
                {
                    var inactiveUsers = await response.Content.ReadFromJsonAsync<List<AppUser>>();
                    return inactiveUsers!;
                }
                else
                {
                    Console.WriteLine($"Failed to fetch inactive users from the API: {response.ReasonPhrase}");
                    return new List<AppUser>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch inactive users from the API: {ex.Message}");
                return new List<AppUser>();
            }
        }

        public void ToggleUserSelection(string userId)
        {
            if (SelectedUserIds.Contains(userId))
            {
                SelectedUserIds.Remove(userId); // Deselect user if already selected
            }
            else
            {
                SelectedUserIds.Add(userId); // Select user if not already selected
            }
        }

        public void ClearSelectedUsers()
        {
            SelectedUserIds.Clear();
        }

        public async Task DeleteSelectedUsers()
        {
            try
            {
                foreach (var userId in SelectedUserIds)
                {
                    var response = await _httpClient.DeleteAsync($"api/user/{userId}");

                    if (response.IsSuccessStatusCode)
                    {
                        // Remove the deleted user from the local list
                        Users.RemoveAll(u => u.Id == userId);
                    }
                    else
                    {
                        // Handle the case where deletion fails
                        Console.WriteLine($"Failed to delete user with ID {userId}. Reason: {response.ReasonPhrase}");
                    }
                }

                // Clear the list of selected user IDs
                ClearSelectedUsers();
            }
            catch (Exception ex)
            {
                // Handle the error, such as displaying an error message
                Console.WriteLine($"An error occurred while deleting selected users: {ex.Message}");
            }
        }


        internal async Task RefreshCache()
        {
            try
            {
                // Fetch the latest user data from the API
                var response = await _httpClient.GetAsync("api/user");

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content to a list of AppUser
                    var usersJson = await response.Content.ReadAsStringAsync();
                    var appUsers = JsonSerializer.Deserialize<List<AppUser>>(usersJson);

                    // Update the cache with the latest data
                    Users = appUsers!;

                    Console.WriteLine("Data retrieved from the API and cached successfully.");
                }
                else
                {
                    // Handle the case where the API request fails
                    Console.WriteLine($"Failed to refresh cache: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to refresh cache: {ex.Message}");
                // Handle or log the error as needed
            }
        }


        public async Task<bool> UpdateUserOnServerAndRefreshCache(AppUser user)
        {
            try
            {
                if (user.Id == null)
                {
                    Console.WriteLine("User ID is null.");
                    return false;
                }

                // Serialize the updated user object to JSON
                var json = JsonSerializer.Serialize(user);

                // Create a StringContent object with the JSON data
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send a PUT request to update the user on the server
                var response = await _httpClient.PutAsync($"api/user/{user.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    // If the update was successful, refresh the cache
                    await RefreshCache();
                    return true;
                }
                else
                {
                    // Handle the case where the API request fails
                    Console.WriteLine($"Failed to update user: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update user and refresh cache: {ex.Message}");
                return false;
            }
        }


        // Method to change sorting criteria
        internal async Task ChangeSorting(string sortBy, bool ascendingOrder)
        {
            try
            {
                SortBy = sortBy;
                AscendingOrder = ascendingOrder;

                // Construct the URL for sorting users based on the sorting criteria
                var url = $"api/user?sortBy={sortBy}&ascendingOrder={ascendingOrder}&page={CurrentPage}";

                // Send a GET request to the API to retrieve sorted users
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content to get the sorted users
                    var sortedUsers = await response.Content.ReadFromJsonAsync<List<AppUser>>();

                    if (sortedUsers != null)
                    {
                        // Update the Users list with the sorted users
                        Users = sortedUsers;
                    }
                    else
                    {
                        Console.WriteLine("Failed to deserialize sorted users from the API response.");
                    }
                }
                else
                {
                    // Handle the case where the API request fails
                    Console.WriteLine($"Failed to retrieve sorted users: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while changing sorting criteria: {ex.Message}");
            }
        }


        // Increment the view count for the specified user ID
        public async Task IncrementUserViewCount(string? userId)
        {
            if (userId != null)
            {
                if (_userViewCounts.ContainsKey(userId))
                {
                    _userViewCounts[userId]++;
                }
                else
                {
                    _userViewCounts[userId] = 1; // Initialize view count to 1 for new user
                }
            }
            else
            {
                // Handle the case where userId is null (if needed)
            }

            await Task.CompletedTask; // Await a completed task
        }

        // Get the view count for the specified user ID
        public int GetUserViewCount(string userId)
        {
            return _userViewCounts.ContainsKey(userId) ? _userViewCounts[userId] : 0;
        }

        // Dictionary to store edit counts for users
        private Dictionary<string, int> _userEditCounts = new Dictionary<string, int>();

        // Increment the edit count for the specified user ID
        public async Task IncrementUserEditCount(string? userId)
        {
            if (userId != null)
            {
                if (_userEditCounts.ContainsKey(userId))
                {
                    _userEditCounts[userId]++;
                }
                else
                {
                    _userEditCounts[userId] = 1; // Initialize edit count to 1 for new user
                }
            }
            else
            {
                // Handle the case where userId is null (if needed)
            }

            await Task.CompletedTask; // Await a completed task
        }

        // Get the edit count for the specified user ID
        public int GetUserEditCount(string userId)
        {
            return _userEditCounts.ContainsKey(userId) ? _userEditCounts[userId] : 0;
        }
    }
}




