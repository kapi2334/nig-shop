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
app.MapGet("/api/userService/users/all",async(AppDbContext db)=>
{
    try{
        var users = await db.clients.ToListAsync();
        return Results.Ok(users);
    }
    catch(Exception e){
        //When db is unavaiable...
        Console.WriteLine($"Fatal error during proccess of connecting to dataBase:{e.Message}");
        return Results.Problem(
            detail: "Can't connect to userService database.",
            statusCode: StatusCodes.Status503ServiceUnavailable
        );
    }
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
    try{
        Console.WriteLine($"User deleted: {inputId}");
        return Results.Ok($"Successfully deleted user with ID: {inputId}");
    }
    catch(Exception ex)
    {
        //When db is not responding...
        throw new System.NotImplementedException();
        

    }
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


