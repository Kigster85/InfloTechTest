using NewUserManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using NewUserManagement.Server.Data;
using NewUserManagement.Client.Components.Public.Shared;
using Microsoft.EntityFrameworkCore;
using static NewUserManagement.Client.Services.LoggingClientService;

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
    private AppUser? newUser { get; set; }
    private bool isLoading { get; set; } = true; // Flag to indicate loading state

    [Parameter] public string? UserId { get; set; }
    private List<AppUser> _users = new List<AppUser>();
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
    public async Task<AppUser> GetUserById(string userId)
    {
        try
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            if (appUser != null)
            {
                // Map AppUser to User model or return AppUser directly, depending on your requirements
                return new AppUser
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


    // Method to retrieve all users
    public async Task<List<AppUser>> GetAllUsers()
    {
        try
        {
            // Fetch all users using Identity UserManager
            var appUsers = await _userManager.Users.ToListAsync();

            // Update the cache with the fetched users
            await _DatabaseCache.RefreshCache();

            // Return the fetched users
            return appUsers;
        }
        catch (Exception ex)
        {
            // Log or handle the exception
            _logger.LogError(ex, "Failed to fetch users from the database.");
            // Optionally, show an error message to the user
            Console.WriteLine("Failed to fetch users from the database. Please try again or contact a system administrator.");
            // You might want to re-throw the exception to propagate it to the caller
            throw;
        }
    }
    // Method to update user details
    public async Task<bool> UpdateUser(AppUser user)
    {
        try
        {
            // Retrieve the user from the database using UserManager
            var appUser = await _userManager.FindByIdAsync(user.Id);
            if (appUser != null)
            {
                // Update the properties of the retrieved AppUser with the new values
                appUser.Forename = user.Forename;
                appUser.Surname = user.Surname;
                appUser.Email = user.Email;

                // Update the user in the Identity database
                var result = await _userManager.UpdateAsync(appUser);

                if (result.Succeeded)
                {
                    // If the update was successful, refresh the cache
                    await _DatabaseCache.RefreshCache();
                    // Log the user edit
                    await LogUserEdit(user.Id, "UserEdited", $"User {user.Forename} {user.Surname} details edited.");
                    return true;
                }
                else
                {
                    // If the update failed, handle the error (e.g., log it, display a message)
                    Console.WriteLine("Failed to update user. Please try again.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine($"User with ID {user.Id} not found.");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false;
        }
    }
    public async Task LogUserEdit(string userId, string action, string details)
    {
        try
        {
            // Retrieve the current edit count for the user
            int editCount = _DatabaseCache.GetUserEditCount(userId);

            // Create a log entry
            var logEntry = new LoggingClientService.LogEntry
            {
                Timestamp = DateTime.UtcNow,
                UserId = userId,
                Action = action, // Use the provided action parameter
                Details = details, // Use the provided details parameter
                ViewCount = 0, // Assuming view count is not applicable for edit events
                EditCount = editCount + 1 // Increment the edit count
            };

            // Add the log entry using the log service
            await _logClientService.AddLogEntryAsync(logEntry);

            // Optionally, log a message indicating successful logging
            _logger.LogInformation("User edit logged successfully.");
        }
        catch (Exception ex)
        {
            // Log the exception with additional context
            _logger.LogError(ex, "Error occurred while logging user edit for UserID: {UserID}, Action: {Action}, Details: {Details}", userId, action, details);
            throw; // Re-throw the exception to propagate it to the caller
        }
    }

    // Method to delete a user by ID
    public async Task<bool> DeleteUser(string userId)
    {
        try
        {
            var userToDelete = await _userManager.FindByIdAsync(userId);
            if (userToDelete == null)
            {
                Console.WriteLine($"User with ID {userId} not found.");
                return false;
            }

            var result = await _userManager.DeleteAsync(userToDelete);

            if (result.Succeeded)
            {
                // Log the user deletion
                await LogDeletionAction(userId);

                // Refresh the cache
                await _DatabaseCache.RefreshCache();

                Console.WriteLine($"User with ID {userId} deleted successfully.");

                return true;
            }
            else
            {
                Console.WriteLine($"Failed to delete user with ID {userId}: {string.Join(",", result.Errors)}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while deleting user with ID {userId}: {ex.Message}");
            return false;
        }
    }

    public async Task LogDeletionAction(string userId)
    {
        // Create a log entry for the deletion action
        try
        {
            var logEntry = new LogEntry()
            {
                UserId = userId,
                Action = "UserDeletion",
                Timestamp = DateTime.Now,
                IsDeletedUserEntry = true

            };

            // Call the logging service to store the log entry
            await _logClientService.AddLogEntryAsync(logEntry);
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during logging
            Console.WriteLine($"An error occurred while logging the deletion action for user with ID {userId}: {ex.Message}");
        }
    }

    public async Task<bool> DeleteMultiple(IEnumerable<string> userIds)
    {
        try
        {
            foreach (var userId in userIds)
            {
                var deleted = await DeleteUser(userId);
                if (!deleted)
                {
                    // Log or handle the failure to delete a user
                    Console.WriteLine($"Failed to delete user with ID {userId}. Moving on to the next user...");
                }
            }

            // Log success message or handle as needed
            Console.WriteLine($"Deleted {userIds.Count()} users successfully.");
            return true;
        }
        catch (Exception ex)
        {
            // Handle or log the exception
            Console.WriteLine($"An error occurred while deleting multiple users: {ex.Message}");
            return false;
        }
    }

    public async Task<List<AppUser>> GetActiveUsers()
    {
        // Fetch all users from the database and filter by active status
        var activeUsers = await _userManager.Users.Where(u => u.IsActive).ToListAsync();
        return activeUsers;
    }

    public async Task<List<AppUser>> GetInactiveUsers()
    {
        // Fetch all users from the database and filter by inactive status
        var inactiveUsers = await _userManager.Users.Where(u => !u.IsActive).ToListAsync();
        return inactiveUsers;
    }

}

