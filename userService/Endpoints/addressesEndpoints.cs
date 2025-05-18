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
                    var items = await db.Address.ToListAsync();

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

                    var item = await db.Address.FirstOrDefaultAsync(a => a.id == inputId);

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
                    var address = await db.Address.FindAsync(inputId);
                    if (address is not null)
                    {
                        db.Address.Remove(address);
                        await db.SaveChangesAsync();
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
                        .Build();

                        var entry = db.Address.Add(newAddress);
                        await db.SaveChangesAsync();
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
