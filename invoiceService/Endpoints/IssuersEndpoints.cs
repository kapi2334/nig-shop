using InvoiceService.Models;
using InvoiceService.Data;
using Microsoft.EntityFrameworkCore;
namespace InvoiceService.Endpoints{

    internal static class IssuersEndpoints{
        public static IEndpointRouteBuilder MapIssuersEndpoints(this IEndpointRouteBuilder endpoints){
				 //////////////ISSUER/////////////////////////
            //GET
            endpoints.MapGet("/issuer", async (AppDbContext db) =>
            {
                if (!db.Database.CanConnect())
                {
                    return Results.Problem(
                        detail: $"Can't connect to invoiceService database. Connection: {db.Database.CanConnect()}",
                        statusCode: StatusCodes.Status503ServiceUnavailable
                    );
                }
                    var items = db.Issuer
                    .OrderByDescending(x => x.id)
                    .FirstOrDefault();
                if (items is not null)
                {
                    return Results.Ok(items);
                }
                else
                {
                    return Results.NotFound();
                }
            })
            .WithName("GetIssuer")
            .WithOpenApi();

            endpoints.MapPost("/issuer", async (IssuerDto input, AppDbContext db) =>
            {
                if (!db.Database.CanConnect())
                {
                    return Results.Problem(
                        detail: $"Can't connect to invoiceService database. Connection: {db.Database.CanConnect()}",
                        statusCode: StatusCodes.Status503ServiceUnavailable
                    );
                }
                Issuer mappedIssuer = input.MapToIssuer();
                var newIssuer = db.Issuer.Add(mappedIssuer); 
                if(newIssuer is not null){
                    await db.SaveChangesAsync();    
                    return Results.Ok($"Successfully added new issuer to db with id: {newIssuer.Entity.id}. Provided data will be used in newly created invoices.");
                }
                else{
                    return Results.Problem(
                            statusCode: StatusCodes.Status500InternalServerError,
                            detail: "Failed to add new issuer entry to database. "
                            );
                }
                
            })
            .WithName("AddIssuer")
            .WithOpenApi();
            return endpoints;
        }


    }


}
