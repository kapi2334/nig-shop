using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;
using UserService.Models.Interfaces;
using UserService.Models.Abstract;

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
        var users = await db.users.ToListAsync();
        return Results.Ok(users);
    }
    catch(Exception e){
        return MyResults.dbIsNotAvaiable(e.Message);
   }
})
.WithName("getAllUsers")
.WithOpenApi();
//Getting user data from db

app.MapGet("/api/userService/users/{inputId}", async(int inputId, AppDbContext db) =>
{
    //Variable to save choosen in ticket user.
    //UserType outputUser = null;
    try{
        //Getting users from db
        IEnumerable<UserType> users = await db.users.ToListAsync();
        //Gets first user that has id = inputId
        var outputUser = db.users.FindAsync(inputId); 
        if(outputUser != null){
            return Results.Ok(outputUser);
        }
        else
        {
            return Results.NotFound();
        }
    }
    catch(Exception e){
        //When db is unavaiable...
        return MyResults.dbIsNotAvaiable(e.Message);
    }

})
.WithName("addUserToDataBase")
.WithOpenApi();

//removing user from db
app.MapDelete("/api/userService/users/{inputId}", async(int inputId, AppDbContext db) =>
{
    //UserType userToDelete;
    try{
        var userToDelete = await db.users.FindAsync(inputId);
        if(userToDelete is not null){
            db.users.Remove(userToDelete);
            db.SaveChangesAsync();
            return Results.Ok($"User {inputId} successfully deleted. Changes has been saved to userService database.");
        }else{

            return Results.NotFound();

        }
    }
    catch(Exception ex){
        //When db is not responding...
        return MyResults.dbIsNotAvaiable(ex.Message); 
    }
})
.WithName("removeUserFromDataBase")
.WithOpenApi();

//Adding new user into database 
app.MapPost("/api/userService/users", () =>
{
    return MyResults.dbIsNotAvaiable(" "); 
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();


