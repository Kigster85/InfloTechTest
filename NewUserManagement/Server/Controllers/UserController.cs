using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewUserManagement.Server.Data;
using NewUserManagement.Shared.DTOs;
using NewUserManagement.Shared.Models;

namespace NewUserManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDBContext _dbContext;

        public UserController(AppDBContext appDBContext)
        {
            _dbContext = appDBContext;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers(int page, int pageSize)
        {
            var users = await _dbContext.Users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Forename = u.Forename,
                    Surname = u.Surname,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    DateOfBirth = u.DateOfBirth
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/User/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetActiveUsers(int page, int pageSize)
        {
            var activeUsers = await _dbContext.Users
                .Where(u => u.IsActive)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Forename = u.Forename,
                    Surname = u.Surname,
                    Email = u.Email,
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
            var inactiveUsers = await _dbContext.Users
                .Where(u => !u.IsActive)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Forename = u.Forename,
                    Surname = u.Surname,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    DateOfBirth = u.DateOfBirth
                })
                .ToListAsync();

            return Ok(inactiveUsers);
        }

        // GET: api/User/{Id}
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserById([FromRoute] int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var userDTO = new UserDTO
            {
                Id = user.Id,
                Forename = user.Forename,
                Surname = user.Surname,
                Email = user.Email,
                IsActive = user.IsActive,
                DateOfBirth = user.DateOfBirth
            };

            return Ok(userDTO);
        }
        // PUT: api/User/{userId}
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserDTO userDTO)
        {
            if (userId != userDTO.Id)
            {
                return BadRequest();
            }

            var user = await _dbContext.Users.FindAsync(userId);
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
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<UserDTO>> AddUser([FromBody] UserDTO userDTO)
        {
            // Assign a unique ID to the user
            userDTO.Id = GenerateUniqueId();

            // Map UserDTO to User entity
            var user = new User
            {
                Forename = userDTO.Forename,
                Surname = userDTO.Surname,
                Email = userDTO.Email,
                IsActive = userDTO.IsActive,
                DateOfBirth = userDTO.DateOfBirth
            };

            // Add user to the database
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Return the newly added user
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, userDTO);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                // Get the user to delete from the database
                var userToDelete = await _dbContext.Users.FindAsync(userId);

                // Check if the user exists
                if (userToDelete == null)
                {
                    // User not found, return a not found response
                    return NotFound();
                }

                // Remove the user from the database
                _dbContext.Users.Remove(userToDelete);
                await _dbContext.SaveChangesAsync();

                // Optionally, you can handle a successful deletion (e.g., return a success response)
                return Ok();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the deletion process
                return StatusCode(500, $"An error occurred while deleting user with ID {userId}: {ex.Message}");
            }
        }

        // POST: api/User/delete-multiple
        [HttpPost("delete-multiple")]
        public async Task<ActionResult> DeleteMultipleUsers(List<int> selectedUserIds)
        {
            try
            {
                if (selectedUserIds == null || selectedUserIds.Count == 0)
                {
                    return BadRequest("No user IDs provided for deletion.");
                }

                var usersToDelete = await _dbContext.Users.Where(u => selectedUserIds.Contains(u.Id)).ToListAsync();
                if (usersToDelete == null || usersToDelete.Count == 0)
                {
                    return NotFound("No users found with the provided IDs.");
                }

                _dbContext.Users.RemoveRange(usersToDelete);
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // Helper method to generate a unique ID (you can implement your own logic here)
        private int GenerateUniqueId()
        {
            // Implement your logic to generate a unique ID
            // Example: You can query the database for the maximum ID and add 1 to it
            var maxId = _dbContext.Users.Max(u => u.Id);
            return maxId + 1;
        }
    }

}

