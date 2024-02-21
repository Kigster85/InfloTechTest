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
        [HttpGet("{Id}")]
        public async Task<ActionResult<UserDTO>> GetUserById([FromRoute] int id)
        {
            var user = await _dbContext.Users.FindAsync(id);

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
        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO userDTO)
        {
            if (id != userDTO.Id)
            {
                return BadRequest();
            }

            var user = await _dbContext.Users.FindAsync(id);
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

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDTO>> DeleteUserById(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return Ok();
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
