using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
namespace InvoiceService.Models.Services{
    
internal class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(json, options);
        }
        catch (HttpRequestException ex)
        {
            // Log error or handle accordingly
            Console.WriteLine($"Request error: {ex.Message}");
            return default;
        }
        catch (JsonException ex)
        {
            // Log JSON parsing issues
            Console.WriteLine($"JSON error: {ex.Message}");
            return default;
        }
    }
}


}
