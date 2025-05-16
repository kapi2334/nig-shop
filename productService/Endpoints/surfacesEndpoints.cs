using ProductService.Models;
using ProductService.Data;
namespace ProductService.Endpoints{

    internal static class SurfacesEndpoints{
        public static IEndpointRouteBuilder MapSurfacesEndpoints(this IEndpointRouteBuilder endpoints){
           //////////////SURFACES/////////////////////////
			//GET
			endpoints.MapGet("/surfaces",async (AppDbContext db) =>
					{
					if(!db.Database.CanConnect()){
					return Results.Problem(
							detail: $"Can't connect to productService database.Connection:{db.Database.CanConnect()}",
							statusCode: StatusCodes.Status503ServiceUnavailable
							);
					}
					var items = db.SurfaceTypes.ToList(); 
					if(items is not null){
					return Results.Ok(items);
					}
					else{
					return Results.NotFound();
					} 
					})
			.WithName("GetSurfaces")
			.WithOpenApi();

			//GetSpecified
			endpoints.MapGet("/surfaces/{inputId}",async (int inputId, AppDbContext db) =>
			{
			   if(!db.Database.CanConnect()){
					return Results.Problem(
							   detail: $"Can't connect to productService database.Connection:{db.Database.CanConnect()}",
							   statusCode: StatusCodes.Status503ServiceUnavailable
							   );
			   }
			   var item = await db.SurfaceTypes.FindAsync(inputId);
			   if(item is not null){
				   return Results.Ok(item);
			   } 
			   else{
				   return Results.NotFound();
			   }
			})
			.WithName("GetSurface")
			.WithOpenApi();


			//deleteSpecified
			endpoints.MapDelete("/surfaces/{inputId}",async (int inputId, AppDbContext db) =>
					{
					if(!db.Database.CanConnect()){
					return Results.Problem(
							detail: $"Can't connect to productService database.Connection:{db.Database.CanConnect()}",
							statusCode: StatusCodes.Status503ServiceUnavailable
							);
					}
					var deleted = db.SurfaceTypes.Remove(db.SurfaceTypes.Find(inputId));
					if (deleted is not null){
					db.SaveChangesAsync();
					return Results.Ok(deleted.Entity.id);
					}
					else{
					return Results.NotFound();
					}
					})
			.WithName("deleteSurface")
			.WithOpenApi();


			//POST
			endpoints.MapPost("/surfaces",async (SurfaceType input, AppDbContext db) =>
			{

			   if(!db.Database.CanConnect()){
					return Results.Problem(
							   detail: $"Can't connect to productService database.Connection:{db.Database.CanConnect()}",
							   statusCode: StatusCodes.Status503ServiceUnavailable
							   );
				}
			   try{
					var entry = db.SurfaceTypes.Add(input);   
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
			.WithName("AddSurface")
			.WithOpenApi();


            return endpoints;
        }


    }


}
