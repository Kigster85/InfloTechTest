using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

                var users = await _userManager.Users
                    .Skip((page ?? 1 - 1) * PageSize) // Skip records based on pagination
                    .Take(PageSize) // Take the desired page size
                    .ToListAsync();

                Users = users;
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

        internal async Task<AppUser?> GetUserDetails(string? userId)
        {
            if (userId == null)
                return null;

            // Retrieve the user from the Identity database using UserManager
            return await _userManager.FindByIdAsync(userId);
        }

        internal async Task<List<AppUser>> GetActiveUsers(int page)
        {
            await GetUsersFromDatabaseAndCache(page);

            var startIndex = (page - 1) * PageSize;
            return Users.Where(u => u.IsActive).Skip(startIndex).Take(PageSize).ToList();
        }

        internal async Task<List<AppUser>> GetInactiveUsers(int page)
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
                foreach (var userId in SelectedUserIds)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.DeleteAsync(user);
                        if (result.Succeeded)
                        {
                            // Remove the deleted user from the local list
                            Users.RemoveAll(u => u.Id == userId);
                        }
                        else
                        {
                            // Handle the case where deletion fails
                            Console.WriteLine($"Failed to delete user with ID {userId}. Error: {result.Errors.FirstOrDefault()?.Description}");
                        }
                    }
                    else
                    {
                        // Handle the case where user is not found
                        Console.WriteLine($"User with ID {userId} not found.");
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


        // Sorting properties
        internal string SortBy { get; private set; } = "Id"; // Default sorting by user ID
        internal bool AscendingOrder { get; private set; } = true; // Default ascending order

        private bool _gettingUsersFromDatabaseAndCaching = false;
        private static readonly object lockObject = new object();
        internal async Task RefreshCache()
        {
            try
            {
                // Fetch the latest user data from the Identity database
                var appUsers = await _userManager.Users.ToListAsync();

                // Update the cache with the latest data
                Users = appUsers.ToList();

                Console.WriteLine("Data retrieved from the database and cached successfully.");
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

                var appUser = await _userManager.FindByIdAsync(user.Id);
                if (appUser == null)
                {
                    Console.WriteLine($"User with ID {user.Id} not found.");
                    return false;
                }

                // Update the properties of the retrieved AppUser with the new values
                appUser.Forename = user.Forename;
                appUser.Surname = user.Surname;
                appUser.Email = user.Email;

                // Update the user in the Identity database
                var result = await _userManager.UpdateAsync(appUser);
                if (!result.Succeeded)
                {
                    Console.WriteLine("Failed to update user. Please try again.");
                    return false;
                }

                // If the update was successful, refresh the cache
                await RefreshCache();

                return true;
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




