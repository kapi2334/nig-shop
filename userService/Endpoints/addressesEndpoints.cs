using UserService.Models;
using UserService.Data;

namespace UserService.Endpoints
{
    internal static class addressesEndpoints
    {
        public static IEndpointRouteBuilder MapaddressesEndpoints(this IEndpointRouteBuilder endpoints)
        {
            //////////////Address/////////////////////////
            //GET
            endpoints.MapGet("/address", async (AppDbContext db) =>
                {
                    if (!db.Database.CanConnect())
                    {
                        return Results.Problem(
                            detail: $"Can't connect to userService database. Connection: {db.Database.CanConnect()}",
                            statusCode: StatusCodes.Status503ServiceUnavailable
                        );
                    }
                    var items = db.addresses.ToList();
                    if (items is not null)
                    {
                        return Results.Ok(items);
                    }
                    else
                    {
                        return Results.NotFound();
                    }
                })
                .WithName("Getaddresses")
                .WithOpenApi();

            //GetSpecified
            endpoints.MapGet("/address/{inputId}", async (int inputId, AppDbContext db) =>
                {
                    if (!db.Database.CanConnect())
                    {
                        return Results.Problem(
                            detail: $"Can't connect to userService database. Connection: {db.Database.CanConnect()}",
                            statusCode: StatusCodes.Status503ServiceUnavailable
                        );
                    }
                    var item = await db.addresses.FindAsync(inputId);
                    if (item is not null)
                    {
                        return Results.Ok(item);
                    }
                    else
                    {
                        return Results.NotFound();
                    }
                })
                .WithName("GetAddress")
                .WithOpenApi();

            //deleteSpecified
            endpoints.MapDelete("/address/{inputId}", async (int inputId, AppDbContext db) =>
                {
                    if (!db.Database.CanConnect())
                    {
                        return Results.Problem(
                            detail: $"Can't connect to userService database. Connection: {db.Database.CanConnect()}",
                            statusCode: StatusCodes.Status503ServiceUnavailable
                        );
                    }
                    var deleted = db.addresses.Remove(db.addresses.Find(inputId));
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
                .WithName("DeleteAddress")
                .WithOpenApi();

            //POST
            endpoints.MapPost("/address", async (Address input, AppDbContext db) =>
                {
                    if (!db.Database.CanConnect())
                    {
                        return Results.Problem(
                            detail: $"Can't connect to userService database. Connection: {db.Database.CanConnect()}",
                            statusCode: StatusCodes.Status503ServiceUnavailable
                        );
                    }
                    try
                    {
                        var entry = db.addresses.Add(input);
                        // New occurrence added.
                        db.SaveChangesAsync();
                        // Returning id
                        return Results.Ok(entry.Entity.id);
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem(
                            detail: $"Error during writing data to database: {ex.Message}",
                            statusCode: StatusCodes.Status500InternalServerError
                        );
                    }
                })
                .WithName("AddAddress")
                .WithOpenApi();

            return endpoints;
        }
    }
}
