using UserService.Models;
using UserService.Models.Builders;
using UserService.Data;
using Microsoft.EntityFrameworkCore;

namespace UserService.Endpoints
{
    internal static class addressesEndpoints
    {
        public static IEndpointRouteBuilder MapAddressesEndpoints(this IEndpointRouteBuilder endpoints)
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
                    var items = db.Address
                    .Include(a=>a.User)
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

                    var item = await db.Address
                    .Include(a=>a.User)
                    .FirstOrDefaultAsync(a=>a.id == inputId);

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
                    var deleted = db.Address.Remove(db.Address.Find(inputId));
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
            endpoints.MapPost("/address", async (AddressDto input, AppDbContext db) =>
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
                        Address newAddress = new AddressBuilder()
                        .WithBuildingNo(input.buildingNo)
                        .WithCity(input.city)
                        .WithCountry(input.country)
                        .WithLocaleNo(input.localeNo)
                        .WithPostCode(input.postCode)
                        .WithStreet(input.street)
                        .WithUserId(input.UserId)
                        .Build();

                        var entry = db.Address.Add(newAddress);
                        // New occurrence added.
                        await db.SaveChangesAsync();
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
