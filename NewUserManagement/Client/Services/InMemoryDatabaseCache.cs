using System.Net.Http.Json;
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

        private List<AppUserDTO>? _users = null;
        internal List<AppUserDTO> Users
        {
            get
            {
                return _users ?? new List<AppUserDTO>();
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
                    var users = await response.Content.ReadFromJsonAsync<List<AppUserDTO>>();

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
        public async Task<List<AppUserDTO>> GetAllUsers()
        {
            await GetUsersFromDatabaseAndCache();
            return Users;
        }

        internal async Task<AppUserDTO> GetUserDetails(string userId)
        {
            // Assuming you have a list of users in your cache, you can retrieve the user details by iterating through the list
            return await Task.FromResult(Users.FirstOrDefault(u => u.Id == userId));
        }


        internal async Task<List<AppUserDTO>> GetActiveUsers(int page)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/user/active?page={page}");

                if (response.IsSuccessStatusCode)
                {
                    var activeUsers = await response.Content.ReadFromJsonAsync<List<AppUserDTO>>();
                    return activeUsers!;
                }
                else
                {
                    Console.WriteLine($"Failed to fetch active users from the API: {response.ReasonPhrase}");
                    return new List<AppUserDTO>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch active users from the API: {ex.Message}");
                return new List<AppUserDTO>();
            }
        }

        internal async Task<List<AppUserDTO>> GetInactiveUsers(int page)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/user/inactive?page={page}");

                if (response.IsSuccessStatusCode)
                {
                    var inactiveUsers = await response.Content.ReadFromJsonAsync<List<AppUserDTO>>();
                    return inactiveUsers!;
                }
                else
                {
                    Console.WriteLine($"Failed to fetch inactive users from the API: {response.ReasonPhrase}");
                    return new List<AppUserDTO>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch inactive users from the API: {ex.Message}");
                return new List<AppUserDTO>();
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


        public async Task RefreshCache(HttpClient httpClient)
        {
            try
            {
                // Fetch the latest user data from the server
                var users = await httpClient.GetFromJsonAsync<List<AppUserDTO>>("api/User");

                // Update the cache with the latest data
                lock (lockObject)
                {
                    Users = users?.Select(u => new AppUserDTO
                    {
                        Forename = u.Forename,
                        Surname = u.Surname,
                        Email = u.Email,
                        DateOfBirth = u.DateOfBirth
                    }).ToList() ?? new List<AppUserDTO>();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., network errors, server unreachable)
                Console.WriteLine($"Failed to refresh cache: {ex.Message}");
                // Log or display an error message as needed
            }
        }


        public async Task<bool> UpdateUserOnServerAndRefreshCache(HttpClient httpClient, AppUserDTO user)
        {
            try
            {
                // Send an HTTP request to the server API to update the user data
                HttpResponseMessage response = await httpClient.PutAsJsonAsync($"api/user/{user.Id}", user);

                if (response.IsSuccessStatusCode)
                {
                    // If the update was successful, refresh the cache
                    await RefreshCache(httpClient);
                    return true;
                }
                else
                {
                    // If the update failed, handle the error (e.g., log it, display a message)
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
                    var sortedUsers = await response.Content.ReadFromJsonAsync<List<AppUserDTO>>();

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

        public async Task<AppUserDTO> GetUserDetailsAsync(string userId)
        {
            try
            {
                // Make a call to your API to fetch user details by ID
                var response = await _httpClient.GetAsync($"api/user/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<AppUserDTO>();
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
    }

}




