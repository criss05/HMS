using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;

namespace HMS.DesktopClient.APIClients
{
    public class UserApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5203/api/";

        public UserApiClient()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        // Helper method to create an HttpRequestMessage with the Authorization header
        private HttpRequestMessage CreateRequest(HttpMethod method, string url)
        {
            if (string.IsNullOrEmpty(App.CurrentUser.Token))
                throw new InvalidOperationException("JWT token is missing. Please log in first.");

            var request = new HttpRequestMessage(method, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.CurrentUser.Token);
            return request;
        }


        public async Task<UserWithTokenDto> Login(string email, string password)
        {
            var response = await _httpClient.GetAsync($"User/login?email={email}&password={password}");
            try
            {
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Returned JSON: {json}");
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
                };

                var userWithToken = JsonSerializer.Deserialize<UserWithTokenDto>(json, options);

                if (userWithToken == null)
                {
                    throw new Exception("Login failed. User not found or invalid credentials.");
                }
                return userWithToken;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during login: {ex.Message}");
                return null;
            }
        }

        public async Task<User> GetUserById(int id)
        {
            var request = CreateRequest(HttpMethod.Get, $"User/{id}");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }
            else
            {
                throw new Exception($"Error fetching user: {response.ReasonPhrase}");
            }
        }

    }
}
