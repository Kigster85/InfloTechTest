using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using NewUserManagement.Client;
using NewUserManagement.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using NewUserManagement.Client.Providers;
using Microsoft.AspNetCore.Identity;
using NewUserManagement.Shared.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Register HttpClient with a base address
builder.Services.AddScoped(sp =>
{
    var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    return new HttpClient { BaseAddress = baseAddress };
});




// Register Blazored.LocalStorage
builder.Services.AddBlazoredLocalStorage();
// Register InMemoryDatabaseCache
builder.Services.AddScoped<InMemoryDatabaseCache>();
builder.Services.AddMemoryCache();

// Register services for logging
builder.Services.AddScoped<LoggingService>();
builder.Services.AddScoped<LoggingCache>();
builder.Services.AddScoped<LoggingClientService.ILogService, LoggingClientService.LogService>();
// Register AuthenticationStateProvider and AppAuthStateProvider
builder.Services.AddScoped<AppAuthStateProvider>();
builder.Services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();

builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<AppAuthStateProvider>());
builder.Services.AddAuthorizationCore();
await builder.Build().RunAsync();
