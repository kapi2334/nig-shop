using Microsoft.EntityFrameworkCore;
using UserService.Data;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Getting user data from db

app.MapGet("/api/usersService/users/{inputId}", (int inputId) =>
{
    return Results.Ok($"Called action.Called user {inputId} ");
})
.WithName("addUserToDataBase")
.WithOpenApi();

//removing user from db
app.MapDelete("/api/userService/users/{inputId}", (int inputId) =>
{
    Console.WriteLine($"User deleted: {inputId}");
    return Results.Ok();
})
.WithName("removeUserFromDataBase")
.WithOpenApi();

//Adding new user into database 
app.MapPost("/api/usersService/users", () =>
{

})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();


