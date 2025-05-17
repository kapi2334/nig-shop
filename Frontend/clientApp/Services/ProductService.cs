using System.Net.Http.Json;
using System.Text.Json;
using clientApp.Models;
using Microsoft.Extensions.Logging;

namespace clientApp.Services;

public class ProductService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductService> _logger;

    public ProductService(HttpClient httpClient, ILogger<ProductService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        // Set accept header to application/json
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching products from API");
            
            // Use the nginx proxy endpoint on port 80
            var response = await _httpClient.GetAsync("http://localhost:80/api/products");
            _logger.LogInformation("Response status code: {StatusCode}", response.StatusCode);
            
            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Raw response content: {Content}", content);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"API returned {response.StatusCode}: {content}");
            }

            if (string.IsNullOrEmpty(content))
            {
                _logger.LogWarning("Received empty content from API");
                return new List<Product>();
            }

            // Try parsing with explicit options
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };

            var products = JsonSerializer.Deserialize<List<Product>>(content, options);
            _logger.LogInformation("Successfully fetched {Count} products", products?.Count ?? 0);
            return products ?? new List<Product>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error occurred while fetching products: {Message}", ex.Message);
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON parsing error occurred: {Message} Content: {Content}", ex.Message, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
            throw;
        }
    }
} 