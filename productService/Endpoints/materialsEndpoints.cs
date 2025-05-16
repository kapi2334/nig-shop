using ProduktService.Models;
using ProduktService.Data;
namespace ProduktService.Endpoints{

    internal static class MaterialsEndpoints{
        public static IEndpointRouteBuilder MapMaterialsEndpoints(this IEndpointRouteBuilder endpoints){
           //////////////Material/////////////////////////
			//GET
			endpoints.MapGet("/materials",async (AppDbContext db) =>
					{
					if(!db.Database.CanConnect()){
					return Results.Problem(
							detail: $"Can't connect to productService database.Connection:{db.Database.CanConnect()}",
							statusCode: StatusCodes.Status503ServiceUnavailable
							);
					}
					var items = db.Materials.ToList(); 
					if(items is not null){
					return Results.Ok(items);
					}
					else{
					return Results.NotFound();
					} 
					})
			.WithName("GetMaterials")
			.WithOpenApi();

			//GetSpecified
			endpoints.MapGet("/materials/{inputId}",async (int inputId, AppDbContext db) =>
			{
			   if(!db.Database.CanConnect()){
					return Results.Problem(
							   detail: $"Can't connect to productService database.Connection:{db.Database.CanConnect()}",
							   statusCode: StatusCodes.Status503ServiceUnavailable
							   );
			   }
			   var item = await db.Materials.FindAsync(inputId);
			   if(item is not null){
				   return Results.Ok(item);
			   } 
			   else{
				   return Results.NotFound();
			   }
			})
			.WithName("GetMaterial")
			.WithOpenApi();


			//deleteSpecified
			endpoints.MapDelete("/materials/{inputId}",async (int inputId, AppDbContext db) =>
					{
					if(!db.Database.CanConnect()){
					return Results.Problem(
							detail: $"Can't connect to productService database.Connection:{db.Database.CanConnect()}",
							statusCode: StatusCodes.Status503ServiceUnavailable
							);
					}
					var deleted = db.Materials.Remove(db.Materials.Find(inputId));
					if (deleted is not null){
					db.SaveChangesAsync();
					return Results.Ok();
					}
					else{
					return Results.NotFound();
					}
					})
			.WithName("deleteMaterial")
			.WithOpenApi();


			//POST
			endpoints.MapPost("/materials",async (Material input, AppDbContext db) =>
			{

			   if(!db.Database.CanConnect()){
					return Results.Problem(
							   detail: $"Can't connect to productService database.Connection:{db.Database.CanConnect()}",
							   statusCode: StatusCodes.Status503ServiceUnavailable
							   );
				}
			   try{
					var entry = db.Materials.Add(input);   
					//New occurence added.
					db.SaveChangesAsync();
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
			.WithName("AddMaterial")
			.WithOpenApi();



            return endpoints;
        }


    }


}
