using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HMS.Shared.Proxies.Implementations
{
    internal class DepartmentProxy : IDepartmentRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = Config._base_api_url;
        private readonly string _token;

        public DepartmentProxy(HttpClient httpClient, string token)
        {
            this._httpClient = httpClient;
            this._token = token;
        }

        private void AddAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._token);
        }
        public async Task<Department> AddAsync(Department department)
        {
            try
            {
                AddAuthorizationHeader();
                string departmentJson = JsonSerializer.Serialize(department, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                });
                StringContent content = new StringContent(departmentJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "department", content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Department>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                })!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding department: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();
                HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"department/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting department: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            try
            {
                AddAuthorizationHeader();
                HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "department");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<Department>>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                })!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all departments: {ex.Message}");
                throw;
            }
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();
                HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"department/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Department>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting department by ID: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Department department)
        {
            try
            {
                AddAuthorizationHeader();
                string appointmentJson = JsonSerializer.Serialize(department, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                });
                StringContent content = new StringContent(appointmentJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"department/{department.Id}", content);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating department: {ex.Message}");
                throw;
            }
        }
    }
}
