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


            //GeneratePdf
            endpoints.MapGet("/invoices/{inputId}/pdf", async (int inputId, AppDbContext db) =>
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
                    var item = await db.Invoice
                        .Include(i => i.issuer)
                        .Include(i => i.products)
                        .FirstOrDefaultAsync(i => i.id == inputId);

                    if(item is null)
                    {
                        return Results.NotFound($"Invoice with given ID:{inputId} is not existing.");
                    }

                    Console.WriteLine($"Found invoice. Products count: {item.products?.Count ?? 0}");
                    Console.WriteLine($"Issuer data present: {item.issuer != null}");

                    if (item.issuer == null)
                    {
                        return Results.Problem(
                            detail: "Invoice issuer data is missing",
                            statusCode: StatusCodes.Status500InternalServerError
                        );
                    }

                    if (item.products == null)
                    {
                        return Results.Problem(
                            detail: "Invoice products list is null",
                            statusCode: StatusCodes.Status500InternalServerError
                        );
                    }

                    if (!item.products.Any())
                    {
                        return Results.Problem(
                            detail: "Invoice products list is empty",
                            statusCode: StatusCodes.Status500InternalServerError
                        );
                    }

                    try
                    {
                        Console.WriteLine("Starting PDF generation...");
                        InvoicePdfGeneratorService pdfService = new InvoicePdfGeneratorService();
                        MemoryStream outPdf = pdfService.Generate(item);
                        Console.WriteLine("PDF generated successfully");
                        
                        if (outPdf == null)
                        {
                            return Results.Problem(
                                detail: "PDF generation returned null stream",
                                statusCode: StatusCodes.Status500InternalServerError
                            );
                        }

                        if (outPdf.Length == 0)
                        {
                            return Results.Problem(
                                detail: "Generated PDF is empty",
                                statusCode: StatusCodes.Status500InternalServerError
                            );
                        }

                        return Results.File(outPdf.ToArray(), "application/pdf", $"invoice_{item.id}.pdf");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"PDF Generation error details: {ex}");
                        return Results.Problem(
                            detail: $"Error during generation of PDF file: {ex.Message}\nStack trace: {ex.StackTrace}",
                            statusCode: StatusCodes.Status500InternalServerError
                        );
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"General error details: {ex}");
                    return Results.Problem(
                        detail: $"General error: {ex.Message}\nStack trace: {ex.StackTrace}",
                        statusCode: StatusCodes.Status500InternalServerError
                    );
                }
            })
            .WithName("GetInvoicePdf")
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
                    if (endpoints is not WebApplication app)
                    {
                        return Results.Problem(statusCode: StatusCodes.Status500InternalServerError, detail: "Can't access httpService");
                    }

                    var apiService = app.Services.GetRequiredService<ApiService>();
                    
                    // Get user data
                    string userUri = app.Configuration.GetConnectionString("UserService") + $"users/{input.clientId}";
                    User user = await apiService.GetAsync<User>(userUri);
                    if (user is null)
                    {
                        return Results.Problem(
                            statusCode: StatusCodes.Status500InternalServerError,
                            detail: "Error while obtaining user from UserService."
                        );
                    }

                    // Get user's address
                    string addressUri = app.Configuration.GetConnectionString("UserService") + $"address/{input.addressId}";
                    Address address = await apiService.GetAsync<Address>(addressUri);
                    //Check is given address in reponse belongs to active user
                    bool contains = false;
                    foreach(Address adr in user.Addresses){
                        if(adr.id == address.id){
                            contains = true;
                        }
                    }
                    if(contains == false){
                      return Results.Problem(
                            statusCode: StatusCodes.Status500InternalServerError,
                            detail: "Given address ID is not valid. Its not an address of given user."
                        );
  
                    }
                    if (address is null)
                    {
                        
                        return Results.Problem(
                            statusCode: StatusCodes.Status500InternalServerError,
                            detail: "Error while obtaining user's address from UserService."
                        );
                    }
                    

                    // Get issuer
                    Issuer issuer = await db.Issuer
                        .OrderByDescending(i => i.id)
                        .FirstOrDefaultAsync();

                    if (issuer is null)
                    {
                        return Results.NotFound("Valid issuer data not found. Service may be temporarily unavailable.");
                    }

                    if (input.products == null || !input.products.Any())
                    {
                        return Results.BadRequest("Products list cannot be empty");
                    }

                    // Create invoice from given data
                    Invoice outInvoice = new InvoiceBuilder()
                        .WithDataFromUser(user)
                        .WithDataFromAddress(address)
                        .WithIssueDate(DateTime.Now.ToUniversalTime())
                        .WithIssuer(issuer)
                        .WithIssuerId(issuer.id)
                        .WithPaymentDeadline(DateTime.Now.AddDays(14).ToUniversalTime())
                        .WithPaymentType(input.paymentType)
                        .Build();

                    // Create Products on invoice 
                    await outInvoice.BuildProductInfosAsync(input.products, apiService);

                    // Add and save to database
                    var invEntry = await db.Invoice.AddAsync(outInvoice);
                    await db.SaveChangesAsync();

                    return Results.Ok(invEntry.Entity.id);
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
