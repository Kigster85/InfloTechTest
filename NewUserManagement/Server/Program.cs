using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewUserManagement.Server.Data;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NewUserManagement.Client.Services;
using NewUserManagement.Shared.Utilities;
using NewUserManagement.Shared.Models;




var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information) // Adjust log levels as needed
    .WriteTo.Console() // Log to the console for development
    .WriteTo.SQLite("LogEntries.db", restrictedToMinimumLevel: LogEventLevel.Information, storeTimestampInUtc: true) // Log to SQLite database
    .CreateLogger();

// Replace the default logging configuration with Serilog
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder =>
        {
            builder.WithOrigins("http://localhost:5167")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDefaultIdentity<AppUser>()
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDBContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKeyGenerator = new SecretKeyGenerator(); // Create an instance of SecretKeyGenerator
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyGenerator.GenerateSecretKey()))
        };
    });
builder.Services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, UserClaimsPrincipalFactory<AppUser, IdentityRole>>();
// Register services for logging
builder.Services.AddScoped<LoggingService>();
builder.Services.AddScoped<LoggingCache>();
builder.Services.AddScoped<LoggingClientService.ILogService, LoggingClientService.LogService>();
builder.Services.AddScoped(sp =>
{
    var httpClient = new HttpClient();
    httpClient.BaseAddress = new Uri("https://localhost:5167"); // Replace with your API base URL
    return httpClient;
});
builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<SecretKeyGenerator>();
builder.Services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<AppDBContext>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseCors("AllowLocalhost");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
