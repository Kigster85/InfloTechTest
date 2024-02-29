using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewUserManagement.Client.Static;
using NewUserManagement.Server.Data;
using NewUserManagement.Shared.Models;

namespace NewUserManagement.Client.Services
{
    public sealed class InMemoryDatabaseCache
    {
        private readonly HttpClient _httpClient;
        private readonly UserService _userService;
        private readonly UserManager<AppUser> _userManager;

        public InMemoryDatabaseCache(HttpClient httpClient, UserService userService, UserManager<AppUser> userManager)
        {
            _httpClient = httpClient;
            OnUsersDataChanged += delegate { }; // Initializing with an empty delegate
            _userService = userService;
            _userManager = userManager;
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
        internal int PageSize { get; set; } = 25; // Number of users to fetch per page
        internal int TotalPages { get; private set; }
        internal int CurrentPage { get; private set; }

        // Assuming User is your custom user model and AppUser is the ASP.NET Core Identity user model
        internal async Task GetUsersFromDatabaseAndCache(int? page)
        {
            if (_gettingUsersFromDatabaseAndCaching)
                return;

            try
            {
                _gettingUsersFromDatabaseAndCaching = true;

                // Assuming you're using UserManager<AppUser> userManager
                var users = await _userManager.Users
                    .Skip((page ?? 1 - 1) * PageSize) // Skip records based on pagination
                    .Take(PageSize) // Take the desired page size
                    .ToListAsync();

                // Map AppUser objects to User objects
                var mappedUsers = users.Select(u => new User
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    // Map other properties as needed
                }).ToList();

                Users = mappedUsers;
                CurrentPage = page ?? 1;
                Console.WriteLine("Data retrieved from the database and cached successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch users from the Identity database: {ex.Message}");
                // Handle the error (e.g., display a message to the user)
            }
            finally
            {
                _gettingUsersFromDatabaseAndCaching = false;
            }
        }


        internal async Task<List<User>> GetActiveUsers(int page)
        {
            await GetUsersFromDatabaseAndCache(page);

            var startIndex = (page - 1) * PageSize;
            return Users.Where(u => u.IsActive).Skip(startIndex).Take(PageSize).ToList();
        }

        internal async Task<List<User>> GetInactiveUsers(int page)
        {
            await GetUsersFromDatabaseAndCache(page);

            var startIndex = (page - 1) * PageSize;
            return Users.Where(u => !u.IsActive).Skip(startIndex).Take(PageSize).ToList();
        }

        public List<string> SelectedUserIds { get; private set; } = new List<string>();

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
                // Send a DELETE request to the API endpoint to delete the selected users
                var response = await _httpClient.DeleteAsync($"api/User/delete-multiple?ids={string.Join(",", SelectedUserIds)}");
                response.EnsureSuccessStatusCode();

                // Remove the deleted users from the local list
                Users.RemoveAll(u => u.Id != null && SelectedUserIds.Contains(u.Id));

                // Clear the list of selected user IDs
                ClearSelectedUsers();
            }
            catch (Exception ex)
            {
                // Handle the error, such as displaying an error message
                Console.WriteLine($"An error occurred while deleting selected users: {ex.Message}");
            }
        }

        // Sorting properties
        internal string SortBy { get; private set; } = "Id"; // Default sorting by user ID
        internal bool AscendingOrder { get; private set; } = true; // Default ascending order

        private bool _gettingUsersFromDatabaseAndCaching = false;
        private static readonly object lockObject = new object();
        public async Task RefreshCache(UserManager<AppUser> userManager)
        {
            try
            {
                // Fetch all users using UserManager
                var users = await userManager.Users.ToListAsync();

                // Update the cache with the latest data
                lock (lockObject)
                {
                    Users = users.Select(u => new User
                    {
                        Id = u.Id,
                        Forename = u.Forename,
                        Surname = u.Surname,
                        Email = u?.Email, // Use null-conditional operator to handle possible null reference                        DateOfBirth = u.DateOfBirth
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., network errors, server unreachable)
                Console.WriteLine($"Failed to refresh cache: {ex.Message}");
                // Log or display an error message as needed
            }
        }
        public async Task<bool> UpdateUserOnServerAndRefreshCache(HttpClient httpClient, User user)
        {
            try
            {
                // Send an HTTP request to the server API to update the user data
                HttpResponseMessage response = await httpClient.PutAsJsonAsync($"api/User/{user.Id}", user);

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
        internal async Task<User?> GetUserDetails(string? userId)
        {
            // Assuming you have a list of users in your cache, you can retrieve the user details by iterating through the list
            return await Task.FromResult(Users.FirstOrDefault(u => u.Id == userId));
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

        // Dictionary to store view counts for users
        private Dictionary<string, int> _userViewCounts = new Dictionary<string, int>();

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




