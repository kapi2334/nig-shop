public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
}

public class CartItem
{
    public Product Product { get; set; } = new();
    public int Quantity { get; set; }
}