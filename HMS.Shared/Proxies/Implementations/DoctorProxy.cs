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
    public class DoctorProxy : IDoctorRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = Config._base_api_url;
        private readonly string _token;

        public DoctorProxy(HttpClient httpClient, string token)
        {
            this._httpClient = httpClient;
            this._token = token;
        }

        private void AddAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._token);
        }

        public async Task<Doctor> AddAsync(Doctor doctor)
        {
            AddAuthorizationHeader();
            string doctorJson = JsonSerializer.Serialize(doctor, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
            });
            StringContent content = new StringContent(doctorJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "doctor", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Doctor>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
            })!;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"doctor/{id}");
            response.EnsureSuccessStatusCode();

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "doctor");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            IEnumerable<Doctor> doctors = JsonSerializer.Deserialize<IEnumerable<Doctor>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
               Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
            });

            return doctors;
        }

        public async Task<Doctor> GetByIdAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"doctor/{id}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            Doctor doctor = JsonSerializer.Deserialize<Doctor>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
               Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
            });

            return doctor;
        }

        public async Task<IEnumerable<Doctor>> GetByDepartmentIdAsync(int departmentId)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"doctor/department/{departmentId}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            IEnumerable<Doctor> doctors = JsonSerializer.Deserialize<IEnumerable<Doctor>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
            });

            return doctors;
        }

        public async Task<bool> UpdateAsync(Doctor doctor)
        {
            AddAuthorizationHeader();
            string doctorJson = JsonSerializer.Serialize(doctor, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
            });
            StringContent content = new StringContent(doctorJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"doctor/{doctor.Id}", content);
            response.EnsureSuccessStatusCode();

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            return response.IsSuccessStatusCode;
        }
    }
}
