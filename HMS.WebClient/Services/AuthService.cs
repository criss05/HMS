using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using HMS.Shared.DTOs;
using HMS.Shared.Enums;

namespace HMS.WebClient.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private const string TokenKey = "JWTToken";
        private const string UserKey = "CurrentUser";

        public AuthService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5203/api/";
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(apiBaseUrl);
        }

        public async Task<UserWithTokenDto?> Login(string email, string password)
        {
            try
            {
                var response = await _httpClient.GetAsync($"User/login?email={Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Login failed with status code: {response.StatusCode}");
                    return null;
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Login response: {jsonContent}");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                };

                try
                {
                    var userWithToken = JsonSerializer.Deserialize<UserWithTokenDto>(jsonContent, options);

                    if (userWithToken == null)
                    {
                        Console.WriteLine("Failed to deserialize user data");
                        return null;
                    }

                    _httpContextAccessor.HttpContext?.Session.SetString(TokenKey, userWithToken.Token);
                    _httpContextAccessor.HttpContext?.Session.SetString(UserKey, jsonContent);

                    return userWithToken;
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"JSON deserialization error: {ex.Message}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> Register(UserDto userDto)
        {
            try
            {
                var json = JsonSerializer.Serialize(userDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("User", content);

                Console.WriteLine($"Register response: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                return false;
            }
        }

        public UserWithTokenDto? GetCurrentUser()
        {
            var userJson = _httpContextAccessor.HttpContext?.Session.GetString(UserKey);

            if (string.IsNullOrEmpty(userJson))
                return null;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };

            try
            {
                return JsonSerializer.Deserialize<UserWithTokenDto>(userJson, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing current user: {ex.Message}");
                return null;
            }
        }

        public string? GetToken()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString(TokenKey);
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext?.Session.Remove(TokenKey);
            _httpContextAccessor.HttpContext?.Session.Remove(UserKey);
        }

        public HttpClient CreateAuthorizedClient()
        {
            var client = new HttpClient();
            var token = GetToken();

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            client.BaseAddress = _httpClient.BaseAddress;
            return client;
        }
    }
}