using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace clientApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IJSRuntime _jsRuntime;

        public ApiService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never, // Never ignore any properties
                WriteIndented = true // Makes debug output more readable
            };
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[Debug] Raw JSON response: {json}");

                try
                {
                    var result = JsonSerializer.Deserialize<T>(json, _jsonOptions);
                    Console.WriteLine($"[Debug] Deserialized object type: {typeof(T).Name}");
                    return result;
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"[Debug] JSON Deserialization error details:");
                    Console.WriteLine($"[Debug] Error message: {ex.Message}");
                    Console.WriteLine($"[Debug] Path: {ex.Path}");
                    Console.WriteLine($"[Debug] LineNumber: {ex.LineNumber}");
                    Console.WriteLine($"[Debug] BytePositionInLine: {ex.BytePositionInLine}");
                    throw;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[Debug] HTTP Request error in ApiService: {ex.Message}");
                throw;
            }
        }

        public async Task<T?> PostAsync<T>(string url, object data)
        {
            try
            {
                // Serialize the request data
                var jsonRequest = JsonSerializer.Serialize(data, _jsonOptions);
                Console.WriteLine($"[Debug] Request URL: {url}");
                Console.WriteLine($"[Debug] Request payload (formatted):");
                Console.WriteLine(jsonRequest);

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Add request headers debug
                Console.WriteLine("[Debug] Request headers:");
                foreach (var header in content.Headers)
                {
                    Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }

                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                // Add response debug
                Console.WriteLine($"[Debug] Response status: {response.StatusCode} ({(int)response.StatusCode})");
                Console.WriteLine("[Debug] Response headers:");
                foreach (var header in response.Headers)
                {
                    Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }
                Console.WriteLine($"[Debug] Response content: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(
                        $"Server returned {response.StatusCode}: {responseContent}",
                        null,
                        response.StatusCode
                    );
                }

                if (string.IsNullOrEmpty(responseContent))
                {
                    Console.WriteLine("[Debug] Empty response content");
                    return default;
                }

                try
                {
                    return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"[Debug] Response deserialization error: {ex.Message}");
                    Console.WriteLine($"[Debug] Response content that failed to deserialize: {responseContent}");
                    throw;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[Debug] HTTP Request error in ApiService: {ex.Message}");
                Console.WriteLine($"[Debug] Status code: {ex.StatusCode}");
                throw;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"[Debug] JSON error in ApiService: {ex.Message}");
                throw;
            }
        }

        public async Task SetInLocalStorageAsync<T>(string key, T value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
        }

        public async Task<T> GetFromLocalStorageAsync<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            if (string.IsNullOrEmpty(json))
                return default!;
            
            return JsonSerializer.Deserialize<T>(json) ?? default!;
        }

        public async Task RemoveFromLocalStorageAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

        public async Task DeleteAsync(string url)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(url);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] API call failed: {ex.Message}");
                throw;
            }
        }

        public async Task PutAsync(string url, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(url, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] API call failed: {ex.Message}");
                throw;
            }
        }
    }
} 