namespace ProduktService.Models{
    //This class is used to create a full-fledged product class later. 
    //It eliminates the need to provide all values for material and surface - and allows only their id from the database.
    internal class ProductDto{
       public string name {get;set;}
       public string type {get;set;}
       public double weight{get;set;}  
       public int dimensionsId {get;set;}
       public int materialId {get;set;}
       public int surfaceTypeId{get;set;}
    }


}
