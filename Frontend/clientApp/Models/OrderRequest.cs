using System.Text.Json.Serialization;

namespace clientApp.Models;

public class OrderRequest
{
    [JsonPropertyName("clientId")]
    public int clientId { get; set; }

    [JsonPropertyName("delivery")]
    public bool delivery { get; set; }

    [JsonPropertyName("products")]
    public List<OrderProductRequest> products { get; set; } = new();
}

public class OrderProductRequest
{
    [JsonPropertyName("productId")]
    public int productId { get; set; }

    [JsonPropertyName("quantity")]
    public int quantity { get; set; }
} 