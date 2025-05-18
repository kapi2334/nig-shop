using OrderService.Models;
using OrderService.Data;
using Microsoft.EntityFrameworkCore;

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
                    var items = await db.Order
                        .Include(o => o.products)
                        .ToListAsync();
                    
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
                    var item = await db.Order
                        .Include(o => o.products)
                        .FirstOrDefaultAsync(o => o.id == inputId);
                        
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
                    var item = await db.Order.FindAsync(inputId);
                    if (item is not null)
                    {
                        db.Order.Remove(item);
                        await db.SaveChangesAsync();
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
                        // Create new order without setting the ID
                        var order = new Order
                        {
                            clientId = input.clientId,
                            delivery = input.delivery,
                            orderId = input.orderId
                        };

                        // Add the order
                        db.Order.Add(order);
                        await db.SaveChangesAsync();

                        // Add products if they exist
                        if (input.products != null && input.products.Any())
                        {
                            foreach (var product in input.products)
                            {
                                var orderedProduct = new OrderedProducts
                                {
                                    orderId = order.id,
                                    productId = product.productId,
                                    quantity = product.quantity
                                };
                                db.OrderedProducts.Add(orderedProduct);
                            }
                            await db.SaveChangesAsync();
                        }

                        return Results.Ok(order.id);
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
