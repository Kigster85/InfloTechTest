using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System;
using System.Net.Http;
using Blazored.LocalStorage;
using NewUserManagement.Client;
using NewUserManagement.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<InMemoryDatabaseCache>();

// Add IHttpClientFactory registration
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
