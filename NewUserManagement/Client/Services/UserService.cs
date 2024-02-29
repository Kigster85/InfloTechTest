using NewUserManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using NewUserManagement.Server.Data;
using NewUserManagement.Client.Components.Public.Shared;

namespace NewUserManagement.Client.Services;

public class UserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly HttpClient _httpClient;
    private readonly InMemoryDatabaseCache _DatabaseCache;
    private readonly ILogger<UserService> _logger; // Inject ILogger<UserService>
    private readonly LoggingService _loggingService;
    private readonly LoggingCache _loggingCache;
    private readonly LoggingClientService.ILogService _logClientService; // Add LoggingClientService
    public UserService(HttpClient httpClient, InMemoryDatabaseCache DatabaseCache, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<UserService> logger, LoggingService loggingService, LoggingCache loggingCache, LoggingClientService.ILogService logClientService)
    {
        _httpClient = httpClient;
        _DatabaseCache = DatabaseCache;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _loggingService = loggingService;
        _loggingCache = loggingCache;
        _logClientService = logClientService;
    }
    private User? newUser { get; set; }
    [Parameter] public string? UserId { get; set; }
    private List<User> _users = new List<User>();
    // Method to create a new user
    public async Task CreateUser(User user, Func<Task> onSuccess, Func<Task> onFailure)
    {
        var appUser = new AppUser
        {
            UserName = user.Email,
            Email = user.Email,
            Forename = user.Forename,
            Surname = user.Surname,
            // No need to set the password here
        };

        var result = await _userManager.CreateAsync(appUser);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(appUser, "Member");

            // Log user creation with the 'Member' role
            await LogUserCreation(appUser.Id, "UserAdded", $"User {appUser.Forename} {appUser.Surname} successfully added to the database with the 'Member' role.");

            await onSuccess.Invoke();
        }
        else
        {
            // Log user creation failure
            Console.WriteLine("Failed to add user. Please try again.");
            await onFailure.Invoke();
        }
    }

    public async Task LogUserCreation(string userId, string action, string details)
    {
        try
        {
            // Create a log entry for the user creation action
            var logEntry = new LoggingClientService.LogEntry
            {
                Timestamp = DateTime.UtcNow,
                UserId = userId,
                Action = action,
                Details = details
            };

            // Call the method to add the log entry
            await _logClientService.AddLogEntryAsync(logEntry);

            // Optionally, you can log a message indicating that the user creation was successfully logged
            _logger.LogInformation("User creation logged successfully.");
        }
        catch (Exception ex)
        {
            // Log or handle any exceptions that occur during user creation logging
            _logger.LogError(ex, "Error occurred while logging user creation: {ErrorMessage}", ex.Message);
            throw; // Re-throw the exception to propagate it to the caller
        }
    }
    
    // Method to retrieve a user by ID
    public async Task<User> GetUserById(string userId)
    {
        try
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            if (appUser != null)
            {
                // Map AppUser to User model or return AppUser directly, depending on your requirements
                return new User
                {
                    Id = appUser.Id,
                    Forename = appUser.Forename,
                    Surname = appUser.Surname,
                    Email = appUser?.Email ?? string.Empty,
                    // Map other properties as needed
                };
            }
            else
            {
                _logger.LogWarning("User not found with ID: {UserId}", userId);
                return null!;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching user by ID: {UserId}", userId);
            throw;
        }
    }
}

// Method to retrieve all users
    public async Task<List<User>> GetAllUsers()
    {
    isLoading = true; // Set loading flag to true
    try
    {
        // Fetch all users using Identity UserManager
        var users = await _userManager.Users.ToListAsync();

        // Update the cache with the fetched users
        await DatabaseCache.UpdateUsersInCache(users);
    }
    catch (Exception ex)
    {
        // Log or handle the exception
        Logger.LogError(ex, "Failed to fetch users from the database.");
        // Optionally, show an error message to the user
        Console.WriteLine("Failed to fetch users from the database. Please try again or contact a system administrator.");
    }
    finally
    {
        isLoading = false; // Set loading flag to false when loading completes or fails
    }
}

//    // Method to update user details
//    public async Task UpdateUser(User user)
//    {
//        // Implement the logic to send a request to the server to update user details
//        // This might involve making a PUT request to an API endpoint
//        // Example:
//        // await HttpClient.PutAsync($"api/users/{user.Id}", user);
//    }

//    // Method to delete a user by ID
//    public async Task DeleteUser(string userId)
//    {
//        // Implement the logic to send a request to the server to delete a user by ID
//        // This might involve making a DELETE request to an API endpoint
//        // Example:
//        // await HttpClient.DeleteAsync($"api/users/{userId}");
//    }
//}
