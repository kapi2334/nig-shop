namespace clientApp.Models
{
    public class ProductDto
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
        public decimal weight { get; set; }
        public decimal price { get; set; }
        public decimal tax { get; set; }
        public DimensionsDto dimensions { get; set; } = new DimensionsDto();
        public MaterialDto material { get; set; } = new MaterialDto();
        public SurfaceTypeDto surfaceType { get; set; } = new SurfaceTypeDto();
    }

    public class DimensionsDto
    {
        public decimal length { get; set; }
        public decimal width { get; set; }
        public decimal height { get; set; }
    }

    public class MaterialDto
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
    }

    public class SurfaceTypeDto
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
    }
} 