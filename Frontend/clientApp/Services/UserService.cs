using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;
using clientApp.Models;
using Microsoft.AspNetCore.Components;
using System.Text;
using System.Net;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace clientApp.Services
{
    public class UserService
    {
        private const string USER_SERVICE_URL = "http://localhost:3001/";  // Using localhost with exposed port

        private readonly ApiService _apiService;
        private readonly NavigationManager _navigationManager;
        private User? _currentUser;

        private class Credentials
        {
            [JsonPropertyName("login")]
            public string login { get; set; } = string.Empty;

            [JsonPropertyName("password")]
            public string password { get; set; } = string.Empty;
        }

        public UserService(HttpClient httpClient, NavigationManager navigationManager, IJSRuntime jsRuntime)
        {
            _apiService = new ApiService(httpClient, jsRuntime);
            _navigationManager = navigationManager;
        }

        public User? CurrentUser => _currentUser;

        public class LoginResult
        {
            public bool Success { get; set; }
            public string ErrorMessage { get; set; } = string.Empty;
        }

        public class SignUpResult
        {
            public bool Success { get; set; }
            public string ErrorMessage { get; set; } = string.Empty;
        }

        public async Task<SignUpResult> SignUpAsync(string login, string password, string name, string surname, string? nip, bool isCompany)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("[Debug] Password is empty or whitespace!");
                    return new SignUpResult 
                    { 
                        Success = false,
                        ErrorMessage = "Password cannot be empty."
                    };
                }

                var url = $"{USER_SERVICE_URL}users";
                Console.WriteLine($"[Debug] Creating user with data:");
                Console.WriteLine($"[Debug] - Login: {login}");
                Console.WriteLine($"[Debug] - Password length: {password.Length}");
                Console.WriteLine($"[Debug] - Name: {name}");
                Console.WriteLine($"[Debug] - Surname: {surname}");
                Console.WriteLine($"[Debug] - IsCompany: {isCompany}");
                Console.WriteLine($"[Debug] - NIP: {(isCompany ? nip ?? "null" : "null")}");
                
                long? nipValue = null;
                if (isCompany && !string.IsNullOrWhiteSpace(nip))
                {
                    if (!long.TryParse(nip, out long parsedNip))
                    {
                        return new SignUpResult 
                        { 
                            Success = false,
                            ErrorMessage = "Invalid NIP format. Please enter a valid number."
                        };
                    }
                    nipValue = parsedNip;
                }

                var user = new User 
                { 
                    login = login,
                    passwordHash = password, // Backend will hash this
                    name = name,
                    surname = surname,
                    nip = nipValue,
                    type = isCompany ? 'c' : 'u',
                    Addresses = new List<AddressDto>() // Initialize empty collection
                };

                Console.WriteLine($"[Debug] Attempting registration at: {url}");
                Console.WriteLine($"[Debug] User object: {user}");
                
                var response = await _apiService.PostAsync<int>(url, user);

                if (response > 0)
                {
                    Console.WriteLine($"[Debug] Registration successful. User ID: {response}");
                    return new SignUpResult { Success = true };
                }

                Console.WriteLine("[Debug] Registration failed - received invalid user ID");
                return new SignUpResult 
                { 
                    Success = false,
                    ErrorMessage = "Registration failed. Please try again."
                };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[Debug] Registration failed: {ex.Message}");
                Console.WriteLine($"[Debug] Status code: {ex.StatusCode}");
                
                var errorMessage = ex.StatusCode switch
                {
                    HttpStatusCode.Conflict => "Username already exists.",
                    HttpStatusCode.BadRequest => "Invalid registration data. Please check all fields.",
                    HttpStatusCode.InternalServerError => "Server error occurred. Please try again later.",
                    HttpStatusCode.ServiceUnavailable => "The service is currently unavailable. Please try again later.",
                    _ => $"Registration error: {ex.Message}"
                };

                Console.WriteLine($"[Debug] Returning error message: {errorMessage}");
                return new SignUpResult 
                { 
                    Success = false, 
                    ErrorMessage = errorMessage
                };
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"[Debug] JSON serialization error: {ex.Message}");
                return new SignUpResult 
                { 
                    Success = false, 
                    ErrorMessage = "Error processing registration data. Please try again."
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Debug] Unexpected error during registration: {ex.Message}");
                Console.WriteLine($"[Debug] Error type: {ex.GetType().Name}");
                Console.WriteLine($"[Debug] Stack trace: {ex.StackTrace}");
                
                return new SignUpResult 
                { 
                    Success = false, 
                    ErrorMessage = "An unexpected error occurred. Please try again."
                };
            }
        }

        public async Task<LoginResult> LoginAsync(string login, string password)
        {
            try
            {
                // Add debug log to see exact URL being constructed
                Console.WriteLine($"[Debug] Base URL: {USER_SERVICE_URL}");
                var url = $"{USER_SERVICE_URL}users/login";
                Console.WriteLine($"[Debug] Full login URL: {url}");

                var credentials = new Credentials { login = login, password = password };

                // Add debug log to see credentials being sent
                Console.WriteLine($"[Debug] Sending credentials - login: {login}");
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

        public async Task<User?> GetCurrentUserAsync()
        {
            if (_currentUser != null)
            {
                return _currentUser;
            }

            // Try to load user data from local storage or session
            var userId = await _apiService.GetFromLocalStorageAsync<int>("userId");
            if (userId > 0)
            {
                try
                {
                    await LoadUserData(userId);
                    return _currentUser;
                }
                catch
                {
                    // If loading fails, clear the stored data
                    await _apiService.RemoveFromLocalStorageAsync("userId");
                }
            }

            return null;
        }

        public async Task LogoutAsync()
        {
            _currentUser = null;
            await _apiService.RemoveFromLocalStorageAsync("userId");
            _navigationManager.NavigateTo("/", true);
        }

        private async Task LoadUserData(int userId)
        {
            try
            {
                var url = $"{USER_SERVICE_URL}users/{userId}";
                _currentUser = await _apiService.GetAsync<User>(url);
                
                if (_currentUser != null)
                {
                    await _apiService.SetInLocalStorageAsync("userId", userId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Debug] Error loading user data: {ex.Message}");
                _currentUser = null;
                throw;
            }
        }

        public bool IsAuthenticated => _currentUser != null;

        public async Task<bool> AddAddress(AddressDto address)
        {
            try
            {
                // First, get the current user
                var currentUser = await GetCurrentUserAsync();
                if (currentUser == null)
                {
                    Console.WriteLine("[Error] Cannot add address: User is not logged in");
                    return false;
                }

                // Add the address to the address service
                var url = $"{USER_SERVICE_URL}address";
                var response = await _apiService.PostAsync<int>(url, address);
                
                if (response > 0)
                {
                    // Set the ID from the response
                    address.id = response;

                    // Update the user's addresses collection
                    if (currentUser.Addresses == null)
                    {
                        currentUser.Addresses = new List<AddressDto>();
                    }
                    currentUser.Addresses.Add(address);

                    // Update the user in the backend
                    var updateUrl = $"{USER_SERVICE_URL}users/{currentUser.id}";
                    await _apiService.PutAsync(updateUrl, currentUser);

                    // Update the local user object
                    _currentUser = currentUser;

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to add address: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveAddress(int addressId)
        {
            try
            {
                var url = $"{USER_SERVICE_URL}address/{addressId}";
                await _apiService.DeleteAsync(url);

                // After successfully removing the address, update the current user's addresses
                var user = await GetCurrentUserAsync();
                if (user != null && user.Addresses != null)
                {
                    user.Addresses = user.Addresses.Where(a => a.id != addressId).ToList();
                    await UpdateCurrentUser(user);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to remove address: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> UpdateCurrentUser(User user)
        {
            try
            {
                var url = $"{USER_SERVICE_URL}users/{user.id}";
                await _apiService.PutAsync(url, user);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to update user: {ex.Message}");
                return false;
            }
        }
    }
} 