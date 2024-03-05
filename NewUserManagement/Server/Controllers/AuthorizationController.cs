using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using static NewUserManagement.Client.Services.LoggingClientService;
using Microsoft.IdentityModel.Tokens;
using NewUserManagement.Shared.Utilities;
using NewUserManagement.Shared.Models;

namespace NewUserManagement.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorizationController : ControllerBase
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogService _loggingService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthorizationController> _logger;
    private readonly SecretKeyGenerator _secretKeyGenerator;
    public AuthorizationController(
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        ILogService loggingService,
        IConfiguration configuration,
        ILogger<AuthorizationController> logger,
        SecretKeyGenerator secretKeyGenerator)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _loggingService = loggingService;
        _configuration = configuration;
        _logger = logger;
        _secretKeyGenerator = secretKeyGenerator;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> SignIn([FromBody] AppUserLogIn user)
    {
        string username = user.EmailAddress;
        string password = user.Password;

        _logger.LogInformation("Attempting to sign in user: {username}", username); // Added logger

        Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(username, password, false, false);
        if (signInResult.Succeeded)
        {
            _logger.LogInformation("User {username} signed in successfully", username); // Added logger

            AppUser identityUser = await _userManager.FindByNameAsync(username);
            string JSONWebTokenAsString = await GenerateJSONWebToken(identityUser);
            return Ok(JSONWebTokenAsString);

        }
        else
        {
            _logger.LogWarning("Failed to sign in user: {username}", username); // Added logger
            return Unauthorized();
        }
    }

    [NonAction]
    [ApiExplorerSettings(IgnoreApi = true)]
    private async Task<string> GenerateJSONWebToken(AppUser identityUser)
    {
        string secretKey = _secretKeyGenerator.GenerateSecretKey();
        SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials credentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new()
        {
            new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, identityUser.Id)
        };

        IList<string> roleNames = await _userManager.GetRolesAsync(identityUser);
        claims.AddRange(roleNames.Select(roleName => new Claim(ClaimsIdentity.DefaultRoleClaimType, roleName)));

        JwtSecurityToken jwtSecurityToken = new
        (
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Issuer"],
            claims,
            null,
            expires: DateTime.UtcNow.AddDays(28),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

}
