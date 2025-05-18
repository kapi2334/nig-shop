namespace clientApp.Models;

public class OrderItem
{
    public Product Product { get; set; }
    public int Quantity { get; set; }

    public OrderItem(Product product, int quantity = 1)
    {
        Product = product;
        Quantity = quantity;
    }

    public decimal TotalPrice => Product.Price * Quantity;
} 