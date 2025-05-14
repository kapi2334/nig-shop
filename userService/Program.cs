<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;
using UserService.Data;
=======

>>>>>>> 8a957fab34f1afb2b3e11f3f43522d5a91980e90
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
app.MapGet("/api/userService/users/all",()=>
{
    return Results.Ok(0);
})
.WithName("getAllUsers")
.WithOpenApi();
//Getting user data from db

app.MapGet("/api/userService/users/{inputId}", (int inputId) =>
{
    return Results.Ok($"Called action.Called user {inputId} ");
})
.WithName("addUserToDataBase")
.WithOpenApi();

//removing user from db
app.MapDelete("/api/userService/users/{inputId}", (int inputId) =>
{
    Console.WriteLine($"User deleted: {inputId}");
    return Results.Ok($"Successfully deleted user with ID: {inputId}");
})
.WithName("removeUserFromDataBase")
.WithOpenApi();

//Adding new user into database 
app.MapPost("/api/userService/users", () =>
{

})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();


