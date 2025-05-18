using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components;
using clientApp;
using clientApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient with base address from configuration
builder.Services.AddScoped(sp => 
{
    var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
    // Add default headers
    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
    return httpClient;
});

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddScoped<OrderApiService>();

// Configure logging for WebAssembly
builder.Services.AddLogging(logging => logging.SetMinimumLevel(LogLevel.Debug));

await builder.Build().RunAsync(); 