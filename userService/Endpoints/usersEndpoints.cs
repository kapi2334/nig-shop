using UserService.Models;
using UserService.Data;

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
                    var items = db.Users.ToList();
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
                    var item = await db.Users.FindAsync(inputId);
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

            return endpoints;
        }
    }
}
