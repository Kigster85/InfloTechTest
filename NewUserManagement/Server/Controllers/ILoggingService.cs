﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewUserManagement.Server.Data;
using NewUserManagement.Shared.Models;

namespace NewUserManagement.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogEntriesController : ControllerBase
    {
        private readonly AppDBContext _dbContext;

        public LogEntriesController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogAction(LogDBEntry logEntry)
        {
            try
            {
                // Set timestamp to current UTC time
                logEntry.Timestamp = DateTime.UtcNow;

                // Add log entry to the database
                _dbContext.LogEntries.Add(logEntry);
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while logging: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetLogEntries()
        {
            try
            {
                var logEntries = await _dbContext.LogEntries.ToListAsync();
                return Ok(logEntries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching log entries: {ex.Message}");
            }
        }



    }
}
