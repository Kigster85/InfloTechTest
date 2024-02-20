﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewUserManagement.Server.Data;
using NewUserManagement.Shared.DTOs;


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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _dbContext.Users
                .Select(u => new UserDTO
                {
                    Id = (int)u.Id,
                    Forename = u.Forename,
                    Surname = u.Surname,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    DateOfBirth = u.DateOfBirth
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetActiveUsers()
        {
            var activeUsers = await _dbContext.Users
                .Where(u => u.IsActive)
                .Select(u => new UserDTO
                {
                    Id = (int)u.Id,
                    Forename = u.Forename,
                    Surname = u.Surname,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    DateOfBirth = u.DateOfBirth
                })
                .ToListAsync();

            return Ok(activeUsers);
        }

        [HttpGet("inactive")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetInactiveUsers()
        {
            var inactiveUsers = await _dbContext.Users
                .Where(u => !u.IsActive)
                .Select(u => new UserDTO
                {
                    Id = (int)u.Id,
                    Forename = u.Forename,
                    Surname = u.Surname,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    DateOfBirth = u.DateOfBirth
                })
                .ToListAsync();

            return Ok(inactiveUsers);
        }
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
        // Add more actions as needed for other endpoints
    }
}
