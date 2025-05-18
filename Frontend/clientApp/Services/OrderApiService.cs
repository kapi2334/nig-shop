using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using clientApp.Models;
using Microsoft.Extensions.Logging;

namespace clientApp.Services;

public class OrderApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OrderApiService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public OrderApiService(HttpClient httpClient, ILogger<OrderApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<bool> PlaceOrderAsync(OrderRequest order)
    {
        try
        {
            _logger.LogInformation("Starting order placement...");
            
            // Ensure we're using the correct URL
            var requestUrl = "api/orders";
            _logger.LogInformation("Base Address: {BaseAddress}", _httpClient.BaseAddress);
            _logger.LogInformation("Request URL: {RequestUrl}", requestUrl);

            // Log the full request details
            var json = JsonSerializer.Serialize(order, _jsonOptions);
            _logger.LogInformation("Request Body: {Json}", json);

            // Ensure proper content type
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            // Add request tracing
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = content
            };
            
            _logger.LogInformation("Sending request to: {FullUrl}", request.RequestUri);
            
            // Send request and get response
            var response = await _httpClient.SendAsync(request);
            
            _logger.LogInformation("Response Status Code: {StatusCode}", response.StatusCode);
            _logger.LogInformation("Response Headers: {Headers}", string.Join(", ", response.Headers.Select(h => $"{h.Key}={string.Join(",", h.Value)}")));

            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Response Content: {Content}", responseContent);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to place order. Status: {StatusCode}, Error: {Error}", 
                    response.StatusCode, responseContent);
                return false;
            }

            _logger.LogInformation("Order placed successfully. Response: {Result}", responseContent);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while placing order");
            return false;
        }
    }
} 