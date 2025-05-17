using InvoiceService.Models;
using InvoiceService.Data;

namespace InvoiceService.Endpoints
{
    internal static class InvoicesEndpoints
    {
        public static IEndpointRouteBuilder MapInvoicesEndpoints(this IEndpointRouteBuilder endpoints)
        {
            //////////////Invoice/////////////////////////
            //GET
            endpoints.MapGet("/invoices", async (AppDbContext db) =>
            {
                if (!db.Database.CanConnect())
                {
                    return Results.Problem(
                        detail: $"Can't connect to invoiceService database. Connection: {db.Database.CanConnect()}",
                        statusCode: StatusCodes.Status503ServiceUnavailable
                    );
                }
                var items = db.invoice.ToList();
                if (items is not null)
                {
                    return Results.Ok(items);
                }
                else
                {
                    return Results.NotFound();
                }
            })
            .WithName("GetInvoices")
            .WithOpenApi();

            // GetSpecified
            endpoints.MapGet("/invoices/{inputId}", async (int inputId, AppDbContext db) =>
            {
                if (!db.Database.CanConnect())
                {
                    return Results.Problem(
                        detail: $"Can't connect to invoiceService database. Connection: {db.Database.CanConnect()}",
                        statusCode: StatusCodes.Status503ServiceUnavailable
                    );
                }
                var item = await db.invoice.FindAsync(inputId);
                if (item is not null)
                {
                    return Results.Ok(item);
                }
                else
                {
                    return Results.NotFound();
                }
            })
            .WithName("GetInvoice")
            .WithOpenApi();

            // DeleteSpecified
            endpoints.MapDelete("/invoices/{inputId}", async (int inputId, AppDbContext db) =>
            {
                if (!db.Database.CanConnect())
                {
                    return Results.Problem(
                        detail: $"Can't connect to invoiceService database. Connection: {db.Database.CanConnect()}",
                        statusCode: StatusCodes.Status503ServiceUnavailable
                    );
                }
                var deleted = db.invoice.Remove(db.invoice.Find(inputId));
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
            .WithName("DeleteInvoice")
            .WithOpenApi();

            // POST
            endpoints.MapPost("/invoices", async (Invoice input, AppDbContext db) =>
            {
                if (!db.Database.CanConnect())
                {
                    return Results.Problem(
                        detail: $"Can't connect to invoiceService database. Connection: {db.Database.CanConnect()}",
                        statusCode: StatusCodes.Status503ServiceUnavailable
                    );
                }
                try
                {
                    var entry = db.invoice.Add(input);
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
            .WithName("AddInvoice")
            .WithOpenApi();

            return endpoints;
        }
    }
}
