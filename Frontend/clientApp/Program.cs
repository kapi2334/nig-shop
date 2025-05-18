using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
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

// Register ApiService with IJSRuntime
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddScoped<OrderApiService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<InvoiceService>();

// Add authentication services
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// Configure logging for WebAssembly
builder.Services.AddLogging(logging => logging.SetMinimumLevel(LogLevel.Debug));

await builder.Build().RunAsync(); 