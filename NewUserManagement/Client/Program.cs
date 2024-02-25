using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using NewUserManagement.Client;
using NewUserManagement.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<LoggingService>();
builder.Services.AddScoped<LoggingCache>();

// Register the LoggingClientService and its implementation
builder.Services.AddScoped<LoggingClientService.ILogService, LoggingClientService.LogService>();
// Register HttpClient with a base address
builder.Services.AddScoped(sp =>
{
    var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    return new HttpClient { BaseAddress = baseAddress };
});

// Register InMemoryDatabaseCache as a scoped service
builder.Services.AddScoped<InMemoryDatabaseCache>();


await builder.Build().RunAsync();
