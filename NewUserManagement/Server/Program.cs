using Microsoft.EntityFrameworkCore;
using NewUserManagement.Server.Data;
using Serilog;
using Serilog.Events;

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
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();

var app = builder.Build();

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

app.UseRouting();
app.UseCors("AllowLocalhost");

app.MapRazorPages();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
