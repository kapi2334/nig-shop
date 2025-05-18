using InvoiceService.Models;
using InvoiceService.Models.Services;
using InvoiceService.Models.Builders;
using InvoiceService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


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
                var items = db.Invoice
                .Include(i => i.issuer) 
                .Include(i => i.products) 
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
                var item = await db.Invoice
                .Include(i => i.issuer) 
                .Include(i => i.products) 
                .FirstOrDefaultAsync(i => i.id == inputId);
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
                var deleted = db.Invoice.Remove(db.Invoice.Find(inputId));
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
            endpoints.MapPost("/invoices/", async (InvoiceDto input, AppDbContext db) =>
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
                    if (endpoints is WebApplication app){
                        var apiService = app.Services.GetRequiredService<ApiService>();
                        User user = await apiService.GetAsync<User>(app.Configuration.GetConnectionString("UserService"));
                        Issuer issuer = db.Issuer
                        .OrderByDescending(i=>i.id)
                        .FirstOrDefault();

                        if(user is not null ){
                            if(issuer is null) { return Results.NotFound("Valid issuer data not found. Service may be temporarily unavailable.");}
                        //Create invoice from given data
                        Invoice outInvoice = new InvoiceBuilder()
                            .WithDataFromUser(user)
                            .WithIssueDate(DateTime.Now)
                            .WithIssuer(issuer)
                            .WithIssuerId(issuer.id)
                            .WithPaymentDeadline(DateTime.Now.AddDays(14))
                            .WithPaymentType(input.paymentType)
                            .Build();
                        //Create Products on invoice 
                        await outInvoice.BuildProductInfosAsync(input.products, apiService);
                            return Results.Ok(outInvoice);

                        }else{
                            return Results.Problem(
                                    statusCode: StatusCodes.Status500InternalServerError,
                                    detail: "Error while obtaining user from UserService."
                                    );

                        }
                        
                        



                        return Results.Ok();
                    }else{
                        return Results.Problem(statusCode: StatusCodes.Status500InternalServerError, detail: "Cant access httpService");
                    } 

                    return Results.Ok();
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
