using InvoiceService.Models;
using InvoiceService.Data;
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
                var items = await db.issuer.FindAsync(1);
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

            return endpoints;
        }


    }


}
