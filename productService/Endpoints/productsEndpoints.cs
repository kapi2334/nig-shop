using ProductService.Models;
using ProductService.Models.Builders;
using ProductService.Data;
using Microsoft.EntityFrameworkCore;
namespace ProductService.Endpoints{

    internal static class ProductsEndpoints{
        public static IEndpointRouteBuilder MapProductsEndpoints(this IEndpointRouteBuilder endpoints){
           //////////////PRODUCT/////////////////////////
			//GET
			endpoints.MapGet("/products",async (AppDbContext db) =>
					{
					if(!db.Database.CanConnect()){
					return Results.Problem(
							detail: $"Can't connect to productService database.Connection:{db.Database.CanConnect()}",
							statusCode: StatusCodes.Status503ServiceUnavailable
							);
					}
                    //Obtaining items and restoring entities
					var items = db.Products
                    .Include(p => p.dimensions)
                    .Include(p => p.material)
                    .Include(p => p.surfaceType)
                    .ToList(); 

					if(items is not null){
					return Results.Ok(items);
					}
					else{
					return Results.NotFound();
					} 
					})
			.WithName("GetProducts")
			.WithOpenApi();

			//GetSpecified
			endpoints.MapGet("/products/{inputId}",async (int inputId, AppDbContext db) =>
			{
			   if(!db.Database.CanConnect()){
					return Results.Problem(
							   detail: $"Can't connect to productService database.Connection:{db.Database.CanConnect()}",
							   statusCode: StatusCodes.Status503ServiceUnavailable
							   );
			   }
			   var item = await db.Products
                   .Include(p => p.dimensions)
                   .Include(p => p.material)
                   .Include(p => p.surfaceType)
                   .FirstOrDefaultAsync(p => p.id == inputId);

			   if(item is not null){
				   return Results.Ok(item);
			   } 
			   else{
				   return Results.NotFound();
			   }
			})
			.WithName("GetProduct")
			.WithOpenApi();


			//deleteSpecified
			endpoints.MapDelete("/products/{inputId}",async (int inputId, AppDbContext db) =>
					{
					if(!db.Database.CanConnect()){
					return Results.Problem(
							detail: $"Can't connect to productService database.Connection:{db.Database.CanConnect()}",
							statusCode: StatusCodes.Status503ServiceUnavailable
							);
					}
					var deleted = db.Products.Remove(db.Products.Find(inputId));
					if (deleted is not null){
						db.SaveChangesAsync();
						return Results.Ok(deleted.Entity.id);
					}
					else{
						return Results.NotFound();
					}
					})
			.WithName("deleteProduct")
			.WithOpenApi();


			//POST
			endpoints.MapPost("/products",async (ProductDto input, AppDbContext db) =>
			{

			   if(!db.Database.CanConnect()){
					return Results.Problem(
							   detail: $"Can't connect to productService database.Connection:{db.Database.CanConnect()}",
							   statusCode: StatusCodes.Status503ServiceUnavailable
							   );
				}
			   try{
                    var product = new ProductBuilder(db)
                    .MapFromDto(input)
                    .SetDimentionsFromId(input.dimensionsId)
                    .SetMaterialFromId(input.materialId)
                    .SetSurfaceFromId(input.surfaceTypeId)
                    .Build();
                    
					var entry = db.Products.Add(product);   
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
			.WithName("AddProduct")
			.WithOpenApi();

            return endpoints;
        }


    }


}
