using UserService.Models;
using UserService.Models.Builders;
using UserService.Data;
using UserService.Models.Services;
using Microsoft.EntityFrameworkCore;

namespace UserService.Endpoints
{
    internal static class UsersEndpoints
    {
        public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder endpoints)
        {
            //////////////User/////////////////////////
            //GET
            endpoints.MapGet("/users", async (AppDbContext db) =>
                {
                    if (!db.Database.CanConnect())
                    {
                        return Results.Problem(
                            detail: $"Can't connect to userService database. Connection: {db.Database.CanConnect()}",
                            statusCode: StatusCodes.Status503ServiceUnavailable
                        );
                    }
                    var items = db.Users
                    .ToList();
                    if (items is not null)
                    {
                        return Results.Ok(items);
                    }
                    else
                    {
                        return Results.NotFound();
                    }
                })
                .WithName("GetUsers")
                .WithOpenApi();

            //GetSpecified
            endpoints.MapGet("/users/{inputId}", async (int inputId, AppDbContext db) =>
                {
                    if (!db.Database.CanConnect())
                    {
                        return Results.Problem(
                            detail: $"Can't connect to userService database. Connection: {db.Database.CanConnect()}",
                            statusCode: StatusCodes.Status503ServiceUnavailable
                        );
                    }
                    var item = await db.Users
                    .Include(u=>u.Addresses)
                    .FirstOrDefaultAsync(u=> u.id == inputId);

                    if (item is not null)
                    {
                        return Results.Ok(item);
                    }
                    else
                    {
                        return Results.NotFound();
                    }
                })
                .WithName("GetUser")
                .WithOpenApi();

            //deleteSpecified
            endpoints.MapDelete("/users/{inputId}", async (int inputId, AppDbContext db) =>
                {
                    if (!db.Database.CanConnect())
                    {
                        return Results.Problem(
                            detail: $"Can't connect to userService database. Connection: {db.Database.CanConnect()}",
                            statusCode: StatusCodes.Status503ServiceUnavailable
                        );
                    }
                    var deleted = db.Users.Remove(db.Users.Find(inputId));
                    if (deleted is not null)
                    {
                        db.SaveChangesAsync();
                        return Results.Ok();
                    }
                    else
                    {
                        return Results.NotFound();
                    }
                })
                .WithName("DeleteUser")
                .WithOpenApi();

            //addUsers
            endpoints.MapPost("/users", async (UserDto input, AppDbContext db) =>
                {
                    if (!db.Database.CanConnect())
                    {
                        return Results.Problem(
                            detail: $"Can't connect to userService database. Connection: {db.Database.CanConnect()}",
                            statusCode: StatusCodes.Status503ServiceUnavailable
                        );
                    }
                    
                    try{
                            

                    
                        User newUser = new UserBuilder()
                        .WithLogin(input.login)
                        .WithPasswordHash(input.passwordHash)
                        .WithNip(input.nip)
                        .WithType(input.type)
                        .WithName(input.name)
                        .WithSurname(input.surname)
                        .Build();
                        
                        AddressMapperService mapper = new AddressMapperService();
                        newUser.Addresses = await mapper.MapoutAddressAsync(input.Addresses.ToList(),db);

                        var addedUser = db.Users.Add(newUser);
                        await db.SaveChangesAsync();
                        return Results.Ok(addedUser);

                    }
                    catch(Exception ex){
                        return Results.Problem(
                            statusCode: StatusCodes.Status500InternalServerError,
                            detail: $"Error during adding new user to database: {ex.Message}"
                                );
                    }


                })
                .WithName("addUser")
                .WithOpenApi();


            return endpoints;
        }
    }
}
