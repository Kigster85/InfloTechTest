using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewUserManagement.Shared.Models;

namespace NewUserManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUserDTO>>> GetUsers()
        {
            var users = await _userManager.Users
                .Select(u => new AppUserDTO
                {
                    Id = u.Id,
                    Email = u.Email,
                    Forename = u.Forename,
                    Surname = u.Surname,
                    IsActive = u.IsActive,
                    DateOfBirth = u.DateOfBirth
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/user/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<AppUserDTO>> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var userDTO = new AppUserDTO
            {
                Email = user.Email,
                Forename = user.Forename,
                Surname = user.Surname,
                IsActive = user.IsActive,
                DateOfBirth = user.DateOfBirth
            };

            return Ok(userDTO);
        }

        // GET: api/User/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<AppUserDTO>>> GetActiveUsers(int page, int pageSize)
        {
            var activeUsers = await _userManager.Users
                .Where(u => u.IsActive)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new AppUserDTO
                {
                    Email = u.Email,
                    Forename = u.Forename,
                    Surname = u.Surname,
                    IsActive = u.IsActive,
                    DateOfBirth = u.DateOfBirth
                })
                .ToListAsync();

            return Ok(activeUsers);
        }

        // GET: api/User/inactive
        [HttpGet("inactive")]
        public async Task<ActionResult<IEnumerable<AppUserDTO>>> GetInactiveUsers(int page, int pageSize)
        {
            var inactiveUsers = await _userManager.Users
                .Where(u => !u.IsActive)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new AppUserDTO
                {
                    Email = u.Email,
                    Forename = u.Forename,
                    Surname = u.Surname,
                    IsActive = u.IsActive,
                    DateOfBirth = u.DateOfBirth
                })
                .ToListAsync();

            return Ok(inactiveUsers);
        }

        // PUT: api/user/{userId}
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] AppUser userDTO)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            user.UserName = userDTO.Email;
            user.Forename = userDTO.Forename;
            user.Surname = userDTO.Surname;
            user.Email = userDTO.emailAddress;
            user.emailAddress = userDTO.Email;
            user.IsActive = userDTO.IsActive;
            user.DateOfBirth = userDTO.DateOfBirth;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] AddUserDTO userDTO)
        {
            if (string.IsNullOrWhiteSpace(userDTO.Email) || string.IsNullOrWhiteSpace(userDTO.Password))
            {
                return BadRequest("Email address and password are required.");
            }

            var user = new AppUser
            {
                UserName = userDTO.Email,
                Forename = userDTO.Forename,
                Surname = userDTO.Surname,
                Email = userDTO.Email,
                emailAddress = userDTO.Email,
                IsActive = true,
                DateOfBirth = userDTO.DateOfBirth,
                Password = userDTO.Password
            };

            // Hash the password using UserManager's password hasher
            var hashedPassword = _userManager.PasswordHasher.HashPassword(user, userDTO.Password);
            user.PasswordHash = hashedPassword;

            // Create the user with the hashed password
            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return StatusCode(201);
        }

        // DELETE: api/User/{userId}
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var userToDelete = await _userManager.FindByIdAsync(userId);

            if (userToDelete == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(userToDelete);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        // DELETE: api/User/delete-multiple
        [HttpPost("delete-multiple")]
        public async Task<ActionResult> DeleteMultipleUsers(List<string> selectedUserIds)
        {
            if (selectedUserIds == null || selectedUserIds.Count == 0)
            {
                return BadRequest("No user IDs provided for deletion.");
            }

            var usersToDelete = await _userManager.Users.Where(u => selectedUserIds.Contains(u.Id)).ToListAsync();

            foreach (var user in usersToDelete)
            {
                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }

            return Ok();
        }
    }
}
