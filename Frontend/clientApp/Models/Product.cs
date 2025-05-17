namespace clientApp.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Weight { get; set; }
    public decimal Price { get; set; }
    public decimal Tax { get; set; }
    public int DimensionsId { get; set; }
    public Dimensions Dimensions { get; set; } = new();
    public int MaterialId { get; set; }
    public Material Material { get; set; } = new();
    public int SurfaceTypeId { get; set; }
    public SurfaceType SurfaceType { get; set; } = new();
}

public class Dimensions
{
    public int Id { get; set; }
    public decimal Length { get; set; }
    public decimal Width { get; set; }
    public decimal Height { get; set; }
}

public class Material
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class SurfaceType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
} 