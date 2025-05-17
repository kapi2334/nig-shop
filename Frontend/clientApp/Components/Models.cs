namespace NigShop.Shared.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public double Weight { get; set; }
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
    public double Length { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
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

public class CartItem
{
    public Product Product { get; set; } = new();
    public int Quantity { get; set; }
}