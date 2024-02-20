using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using NewUserManagement.Client;
using NewUserManagement.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.Services.AddBlazoredLocalStorage();
// Use AddHttpClient to register HttpClient with a base address
builder.Services.AddHttpClient();

// Register InMemoryDatabaseCache as a scoped service
builder.Services.AddScoped<InMemoryDatabaseCache>();



await builder.Build().RunAsync();
