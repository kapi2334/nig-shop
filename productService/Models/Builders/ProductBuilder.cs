using ProductService.Data;
namespace ProductService.Models.Builders{

    internal class ProductBuilder{
        private readonly Product _Product = new Product();
        private AppDbContext db {get;set;}
        public ProductBuilder(AppDbContext db){
            this.db = db;
        }

        
        public ProductBuilder MapFromDto(ProductDto dto){
            _Product.name = dto.name;
            _Product.type = dto.type;
            _Product.weight = dto.weight;
            return this;
        } 
        //Sets dimentions params by obtaining Dinemtions item from db by given id. If object is not found - throws exeption.
        public ProductBuilder SetDimentionsFromId(int dimentionsObjectId){
            Dimensions dimentionsObject = db.Dimensions.Find(dimentionsObjectId);
            if(dimentionsObject is not null){
                _Product.dimensionsId = dimentionsObjectId;
                _Product.dimensions = dimentionsObject; 
            }
            else{
                throw new Exception("Dimentions object with given ID not found.");
            }

            return this;
        } 
        

        //Sets material params by obtaining Matierial item from db by given id. If object is not found - throws exeption.
        public ProductBuilder SetMaterialFromId(int materialObjectId){
            Material materialObject = db.Materials.Find(materialObjectId);
            if(materialObject is not null){
                _Product.materialId = materialObjectId;
                _Product.material= materialObject; 
            }
            else{
                throw new Exception("Material object with given ID not found.");
            }

            return this;
        } 

        //Sets surface params by obtaining Surface item from db by given id. If object is not found - throws exeption.
        public ProductBuilder SetSurfaceFromId(int surfaceObjectId){
            SurfaceType surfaceObject = db.SurfaceTypes.Find(surfaceObjectId);
            if(surfaceObject is not null){
                _Product.surfaceTypeId = surfaceObjectId;
                _Product.surfaceType= surfaceObject; 
            }
            else{
                throw new Exception("Surface object with given ID not found.");
            }

            return this;
        } 

        public Product Build(){
            return _Product;
        }


    }


}
