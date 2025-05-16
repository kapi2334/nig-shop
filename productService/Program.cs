using Microsoft.EntityFrameworkCore;
using ProduktService.Data;
using ProduktService.Models;
using ProduktService.Endpoints;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    EnvironmentName = "Development"
});



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
// -> localhost/api/productService
//Api endpoints 
app.MapDimentionsEndpoints();
app.MapMaterialsEndpoints();
app.MapProductsEndpoints();
app.MapSurfacesEndpoints();




app.Run();

