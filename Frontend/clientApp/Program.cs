using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components;
using clientApp;
using clientApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient
builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddLogging(logging => logging.SetMinimumLevel(LogLevel.Debug));

var app = builder.Build();

// Enable console logging for debugging
await app.RunAsync(); 