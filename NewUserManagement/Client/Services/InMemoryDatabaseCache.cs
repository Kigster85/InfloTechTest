﻿using System.Net.Http.Json;
using NewUserManagement.Client.Static;
using NewUserManagement.Shared.DTOs;
using NewUserManagement.Shared.Models;


namespace NewUserManagement.Client.Services
{
    internal sealed class InMemoryDatabaseCache
    {
        private const int PageSize = 25; // Number of users to fetch per page
        private readonly HttpClient _httpClient;

        public InMemoryDatabaseCache(HttpClient httpClient)
        {
            _httpClient = httpClient;
            OnUsersDataChanged += delegate { }; // Initializing with an empty delegate
            _httpClient.BaseAddress = new Uri("https://localhost:5167"); // Set your API base URL here
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
        public List<int> SelectedUserIds { get; private set; } = new List<int>();

        public void ToggleUserSelection(int userId)
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
                Users.RemoveAll(u => SelectedUserIds.Contains(u.Id));

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
        private static readonly object lockObject = new object();
        public async Task RefreshCache(HttpClient httpClient)
        {
            try
            {
                // Fetch the latest user data from the server
                var users = await httpClient.GetFromJsonAsync<List<UserDTO>>("api/User");

                // Update the cache with the latest data
                lock (lockObject)
                {
                    Users = users?.Select(u => new User
                    {
                        Forename = u.Forename,
                        Surname = u.Surname,
                        Email = u.Email,
                        DateOfBirth = u.DateOfBirth
                    }).ToList() ?? new List<User>();
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
        internal async Task<User?> GetUserDetails(int userId)
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
        private Dictionary<int, int> _userViewCounts = new Dictionary<int, int>();

        // Increment the view count for the specified user ID
        public async Task IncrementUserViewCount(int userId)
        {
            if (_userViewCounts.ContainsKey(userId))
            {
                _userViewCounts[userId]++;
            }
            else
            {
                _userViewCounts[userId] = 1; // Initialize view count to 1 for new user
            }

            await Task.CompletedTask; // Await a completed task
        }



        // Get the view count for the specified user ID
        public int GetUserViewCount(int userId)
        {
            return _userViewCounts.ContainsKey(userId) ? _userViewCounts[userId] : 0;
        }

        // Dictionary to store edit counts for users
        private Dictionary<int, int> _userEditCounts = new Dictionary<int, int>();

        // Increment the edit count for the specified user ID
        public async Task IncrementUserEditCount(int userId)
        {
            if (_userEditCounts.ContainsKey(userId))
            {
                _userEditCounts[userId]++;
            }
            else
            {
                _userEditCounts[userId] = 1; // Initialize view count to 1 for new user
            }

            await Task.CompletedTask; // Await a completed task
        }

        // Get the edit count for the specified user ID
        public int GetUserEditCount(int userId)
        {
            return _userEditCounts.ContainsKey(userId) ? _userEditCounts[userId] : 0;
        }
    }
}




