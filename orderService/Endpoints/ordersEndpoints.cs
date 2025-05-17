using OrderService.Models;
using OrderService.Data;

namespace OrderService.Endpoints
{
    internal static class OrdersEndpoints
    {
        public static IEndpointRouteBuilder MapOrdersEndpoints(this IEndpointRouteBuilder endpoints)
        {
            //////////////Order/////////////////////////
            //GET
            endpoints.MapGet("/orders", async (AppDbContext db) =>
                {
                    if (!db.Database.CanConnect())
                    {
                        return Results.Problem(
                            detail: $"Can't connect to orderService database. Connection: {db.Database.CanConnect()}",
                            statusCode: StatusCodes.Status503ServiceUnavailable
                        );
                    }
                    var items = db.Order.ToList();
                    if (items is not null)
                    {
                        return Results.Ok(items);
                    }
                    else
                    {
                        return Results.NotFound();
                    }
                })
                .WithName("GetOrders")
                .WithOpenApi();

            //GetSpecified
            endpoints.MapGet("/orders/{inputId}", async (int inputId, AppDbContext db) =>
                {
                    if (!db.Database.CanConnect())
                    {
                        return Results.Problem(
                            detail: $"Can't connect to orderService database. Connection: {db.Database.CanConnect()}",
                            statusCode: StatusCodes.Status503ServiceUnavailable
                        );
                    }
                    var item = await db.Order.FindAsync(inputId);
                    if (item is not null)
                    {
                        return Results.Ok(item);
                    }
                    else
                    {
                        return Results.NotFound();
                    }
                })
                .WithName("GetOrder")
                .WithOpenApi();

            //deleteSpecified
            endpoints.MapDelete("/orders/{inputId}", async (int inputId, AppDbContext db) =>
                {
                    if (!db.Database.CanConnect())
                    {
                        return Results.Problem(
                            detail: $"Can't connect to orderService database. Connection: {db.Database.CanConnect()}",
                            statusCode: StatusCodes.Status503ServiceUnavailable
                        );
                    }
                    var deleted = db.Order.Remove(db.Order.Find(inputId));
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
                .WithName("DeleteOrder")
                .WithOpenApi();

            //POST
            endpoints.MapPost("/orders", async (Order input, AppDbContext db) =>
                {
                    if (!db.Database.CanConnect())
                    {
                        return Results.Problem(
                            detail: $"Can't connect to orderService database. Connection: {db.Database.CanConnect()}",
                            statusCode: StatusCodes.Status503ServiceUnavailable
                        );
                    }
                    try
                    {
                        var entry = db.Order.Add(input);
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
                .WithName("AddOrder")
                .WithOpenApi();

            return endpoints;
        }
    }
}
