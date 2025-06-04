using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HMS.Shared.Proxies.Implementations
{
    class UserProxy : IUserRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = Config._base_api_url;
        private readonly string _token;

        public UserProxy(HttpClient httpClient, string token)
        {
            this._httpClient = httpClient;
            this._token = token;
        }
        private void AddAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._token);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            AddAuthorizationHeader();

            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "user");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            IEnumerable<UserDto> users = JsonSerializer.Deserialize<IEnumerable<UserDto>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return users;
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            AddAuthorizationHeader();

            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"user/{id}");
            response.EnsureSuccessStatusCode();
            
            string responseBody = await response.Content.ReadAsStringAsync();
            UserDto? user = JsonSerializer.Deserialize<UserDto>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return user;
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            AddAuthorizationHeader();
            
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"user/email/{email}");
            response.EnsureSuccessStatusCode();
            
            string responseBody = await response.Content.ReadAsStringAsync();
            UserDto? user = JsonSerializer.Deserialize<UserDto>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return user;
        }

        public async Task<UserDto> AddAsync(User user)
        {
            AddAuthorizationHeader();

            string userJson = JsonSerializer.Serialize(user);
            StringContent content = new StringContent(userJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "user", content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            UserDto createdUser = JsonSerializer.Deserialize<UserDto>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (createdUser == null)
            {
                throw new Exception("Failed to deserialize created user.");
            }
            return createdUser;
        }
        public async Task<bool> UpdateAsync(User user)
        {
            AddAuthorizationHeader();

            string userJson = JsonSerializer.Serialize(user);
            StringContent content = new StringContent(userJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"user/{user.Id}", content);
            response.EnsureSuccessStatusCode();

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true; // Update successful
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new ArgumentException("Invalid user data provided.");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false; // User not found
            }
            else
            {
                throw new Exception("Failed to update user.");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            AddAuthorizationHeader();

            HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"user/delete/{id}");
            response.EnsureSuccessStatusCode();

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true; // Deletion successful
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false; // User not found
            }
            else
            {
                throw new Exception("Failed to delete user.");
            }
        }
    }
}
