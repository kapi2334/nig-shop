using ProduktService.Models;
using ProduktService.Data;
namespace ProduktService.Endpoints{

    internal static class DimentionsEndpoints{
        public static IEndpointRouteBuilder MapDimentionsEndpoints(this IEndpointRouteBuilder endpoints){
            ////////////////DIMENSIONS///////////////////////
			//GET
			endpoints.MapGet("/dimensions",async (AppDbContext db) =>{
					if(!db.Database.CanConnect()){
					return Results.Problem(
							detail: $"Can't connect to productService database.Connection:{db.Database.CanConnect()}",
							statusCode: StatusCodes.Status503ServiceUnavailable
							);
					}
					var items = db.Dimensions.ToList(); 
					if(items is not null){
					return Results.Ok(items);
					}
					else{
					return Results.NotFound();
					} 
					})
			.WithName("GetDimentions")
			.WithOpenApi();

			//GetSpecified
			endpoints.MapGet("/dimensions/{inputId}",async (int inputId, AppDbContext db) =>
			{
			   if(!db.Database.CanConnect()){
					return Results.Problem(
							   detail: $"Can't connect to productService database.Connection:{db.Database.CanConnect()}",
							   statusCode: StatusCodes.Status503ServiceUnavailable
							   );
			   }
			   var item = await db.Dimensions.FindAsync(inputId);
			   if(item is not null){
				   return Results.Ok(item);
			   } 
			   else{
				   return Results.NotFound();
			   }
			})
			.WithName("GetSingleDimentions")
			.WithOpenApi();


			//deleteSpecified
			endpoints.MapDelete("/dimensions/{inputId}",async (int inputId, AppDbContext db) =>
					{
					if(!db.Database.CanConnect()){
					return Results.Problem(
							detail: $"Can't connect to productService database.Connection:{db.Database.CanConnect()}",
							statusCode: StatusCodes.Status503ServiceUnavailable
							);
					}
					var deleted = db.Dimensions.Remove(db.Dimensions.Find(inputId));
					if (deleted is not null){
					await db.SaveChangesAsync();
					return Results.Ok();
					}
					else{
					return Results.NotFound();
					}
					})
			.WithName("deleteDimention")
			.WithOpenApi();


			//POST
			endpoints.MapPost("/dimensions",async (Dimensions input, AppDbContext db) =>
			{

			   if(!db.Database.CanConnect()){
					return Results.Problem(
							   detail: $"Can't connect to productService database.Connection:{db.Database.CanConnect()}",
							   statusCode: StatusCodes.Status503ServiceUnavailable
							   );
				}
			   try{
					var entry = db.Dimensions.Add(input);   
					//New occurence added.
					await db.SaveChangesAsync();
					//Returing id
					return Results.Ok(entry.Entity.id); 
			   }
			   catch(Exception ex)
			   {
					return Results.Problem(
						detail : $"Error during writing data to database:{ex.Message}",
						statusCode: StatusCodes.Status500InternalServerError
					);
			   }
			})
			.WithName("AddDimentions")
			.WithOpenApi();

            return endpoints;
        }


    }


}
