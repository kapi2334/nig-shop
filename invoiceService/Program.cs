using Microsoft.EntityFrameworkCore;
using InvoiceService.Data;
using InvoiceService.Endpoints;
using InvoiceService.Models.Services;
using PdfSharpCore.Fonts;
using System.IO;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    EnvironmentName = "Development"
});

// Add CORS support
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Initialize PDF fonts
try
{
    PdfFontInitializer.Initialize();
}
catch (Exception ex)
{
    Console.WriteLine($"Critical error during font initialization: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
    throw; // Prevent app from starting if fonts can't be initialized
}

builder.Services.AddHttpClient<ApiService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // <- to wykona migracje
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS
app.UseCors();

app.UseHttpsRedirection();

app.MapInvoicesEndpoints();
app.MapIssuersEndpoints();

app.Run();

