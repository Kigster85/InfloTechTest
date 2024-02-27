using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewUserManagement.Server.Data;
using NewUserManagement.Shared.Models;

namespace NewUserManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public UserController(AppDBContext appDBContext, UserManager<AppUser> userManager)
        {
            _dbContext = appDBContext;
            _userManager = userManager;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserById([FromRoute] string userId)
        {
            if (!int.TryParse(userId, out int id))
            {
                // userId is not a valid integer string
                return BadRequest("Invalid user ID format.");
            }

            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            var userDTO = new UserDTO
            {
                Id = Convert.ToInt32(user.Id), // Convert string to int
                Forename = user.Forename,
                Surname = user.Surname,
                Email = user.Email ?? "Unknown", // If user.Email is null, assign "Unknown"
                IsActive = user.IsActive,
                DateOfBirth = user.DateOfBirth
            };

            return Ok(userDTO);
        }




        // GET: api/User/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetActiveUsers(int page, int pageSize)
        {
            var activeUsers = await _userManager.Users
                .Where(u => u.IsActive)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new User
                {
                    Id = int.Parse(u.Id),
                    Forename = u.Forename,
                    Surname = u.Surname,
                    Email = u.Email ?? "",
                    IsActive = u.IsActive,
                    DateOfBirth = u.DateOfBirth
                })
                .ToListAsync();

            return Ok(activeUsers);
        }

        // GET: api/User/inactive
        [HttpGet("inactive")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetInactiveUsers(int page, int pageSize)
        {
            var inactiveUsers = await _userManager.Users
                .Where(u => !u.IsActive)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new User
                {
                    Id = int.Parse(u.Id),
                    Forename = u.Forename,
                    Surname = u.Surname,
                    Email = u.Email ?? "",
                    IsActive = u.IsActive,
                    DateOfBirth = u.DateOfBirth
                })
                .ToListAsync();

            return Ok(inactiveUsers);
        }

        
        // PUT: api/User/{userId}
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserDTO userDTO)
        {
            if (userId != userDTO.Id.ToString()) // Convert Id to string for comparison
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Update user properties with the new values
            user.Forename = userDTO.Forename;
            user.Surname = userDTO.Surname;
            user.Email = userDTO.Email;
            user.IsActive = userDTO.IsActive;
            user.DateOfBirth = userDTO.DateOfBirth;

            // Save changes to the database
            await _userManager.UpdateAsync(user); // Use UserManager to update the user

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> AddUser([FromBody] UserDTO userDTO)
        {
            // Map UserDTO to User entity
            var user = new AppUser // Change User to AppUser
            {
                Forename = userDTO.Forename,
                Surname = userDTO.Surname,
                UserName = userDTO.Email, // Set email as username
                Email = userDTO.Email,
                IsActive = true, // Set IsActive to a default value
                DateOfBirth = userDTO.DateOfBirth
            };

            // Create the user using UserManager
            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                // If user creation fails, return error messages
                return BadRequest(result.Errors);
            }

            // Instead of returning the userDTO, return the created user object directly
            return Ok(user); // Return the created user
        }


        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId) // Change int to string
        {
            try
            {
                // Get the user to delete using UserManager
                var userToDelete = await _userManager.FindByIdAsync(userId);

                // Check if the user exists
                if (userToDelete == null)
                {
                    // User not found, return a not found response
                    return NotFound();
                }

                // Remove the user using UserManager
                var result = await _userManager.DeleteAsync(userToDelete);

                // Check if the deletion was successful
                if (!result.Succeeded)
                {
                    // If deletion fails, return error messages
                    return BadRequest(result.Errors);
                }

                // Optionally, you can handle a successful deletion (e.g., return a success response)
                return Ok();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the deletion process
                return StatusCode(500, $"An error occurred while deleting user with ID {userId}: {ex.Message}");
            }
        }


        [HttpPost("delete-multiple")]
        public async Task<ActionResult> DeleteMultipleUsers(List<string> selectedUserIds) // Change int to string
        {
            try
            {
                if (selectedUserIds == null || selectedUserIds.Count == 0)
                {
                    return BadRequest("No user IDs provided for deletion.");
                }

                foreach (var userId in selectedUserIds)
                {
                    // Get the user to delete using UserManager
                    var userToDelete = await _userManager.FindByIdAsync(userId);

                    // Check if the user exists
                    if (userToDelete == null)
                    {
                        // User not found, return a not found response or continue with next user
                        continue;
                    }

                    // Remove the user using UserManager
                    var result = await _userManager.DeleteAsync(userToDelete);

                    // If deletion fails, return error messages
                    if (!result.Succeeded)
                    {
                        return BadRequest(result.Errors);
                    }
                }

                // Optionally, you can handle a successful deletion (e.g., return a success response)
                return Ok();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the deletion process
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // Helper method to generate a unique ID (you can implement your own logic here)
        private string GenerateUniqueId()
        {
            // Generate a new unique identifier (GUID)
            return Guid.NewGuid().ToString();
        }

    }

}

