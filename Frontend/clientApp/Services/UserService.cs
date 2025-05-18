using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;
using clientApp.Models;
using Microsoft.AspNetCore.Components;
using System.Text;
using System.Net;

namespace clientApp.Services
{
    public class UserService
    {
        private readonly ApiService _apiService;
        private readonly NavigationManager _navigationManager;
        private User _currentUser;
        private const string USER_SERVICE_URL = "http://localhost:3001/";  // Using localhost with exposed port

        // Add Credentials class to match backend exactly
        private class Credentials
        {
            public string login { get; set; }
            public string password { get; set; }
        }
    
public class ApiService
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

    public async Task<T?> PostAsync<T>(string url, object data)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = null
            };

            var content = new StringContent(
                JsonSerializer.Serialize(data, options),
                Encoding.UTF8,
                "application/json");

            Console.WriteLine($"[Debug] POST Request to: {url}");
            Console.WriteLine($"[Debug] Request payload: {await content.ReadAsStringAsync()}");

            var response = await _httpClient.PostAsync(url, content);
            Console.WriteLine($"[Debug] Response status: {response.StatusCode}");

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[Debug] Response content: {responseContent}");

            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<T>(responseContent, options);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"[Debug] HTTP Request error: {ex.Message}");
            Console.WriteLine($"[Debug] Status code: {ex.StatusCode}");
            throw;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"[Debug] JSON error: {ex.Message}");
            throw;
        }
    }
}





        public User CurrentUser => _currentUser;

        public class LoginResult
        {
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }
        }

        public UserService(HttpClient httpClient, NavigationManager navigationManager)
        {
            _apiService = new ApiService(httpClient);
            _navigationManager = navigationManager;
        }

        public async Task<LoginResult> LoginAsync(string login, string password)
        {
            try
            {
                var url = $"{USER_SERVICE_URL}users/login";
                var credentials = new Credentials { login = login, password = password };

                Console.WriteLine($"[Debug] Attempting login at: {url}");
                var userId = await _apiService.PostAsync<int>(url, credentials);

                if (userId > 0)
                {
                    await LoadUserData(userId);
                    return new LoginResult { Success = true };
                }

                return new LoginResult 
                { 
                    Success = false,
                    ErrorMessage = "Invalid username or password."
                };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[Debug] Login failed: {ex.Message}");
                return new LoginResult 
                { 
                    Success = false, 
                    ErrorMessage = ex.StatusCode switch
                    {
                        HttpStatusCode.Unauthorized => "Invalid username or password.",
                        HttpStatusCode.NotFound => "Invalid username or password.",
                        HttpStatusCode.ServiceUnavailable => "The user service is currently unavailable. Please try again later.",
                        _ => $"Network error: {ex.Message} (Status: {ex.StatusCode})"
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Debug] Unexpected error during login: {ex.Message}");
                return new LoginResult 
                { 
                    Success = false, 
                    ErrorMessage = $"Error: {ex.Message}"
                };
            }
        }

        private async Task LoadUserData(int userId)
        {
            try
            {
                var url = $"{USER_SERVICE_URL}users/{userId}";
                _currentUser = await _apiService.GetAsync<User>(url);
                
                if (_currentUser == null)
                {
                    throw new Exception("Failed to load user data");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Debug] LoadUserData failed: {ex.Message}");
                _currentUser = null;
                throw;
            }
        }

        public void Logout()
        {
            _currentUser = null;
            _navigationManager.NavigateTo("/login");
        }

        public bool IsAuthenticated => _currentUser != null;
    }
} 