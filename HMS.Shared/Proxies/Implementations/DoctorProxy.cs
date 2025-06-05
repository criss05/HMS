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
        private readonly JsonSerializerOptions _jsonOptions;

        public DoctorProxy(HttpClient httpClient, string token)
        {
            this._httpClient = httpClient;
            this._token = token;
            this._jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
        }

        private void AddAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._token);
        }

        public async Task<Doctor> AddAsync(Doctor doctor)
        {
            AddAuthorizationHeader();
            string doctorJson = JsonSerializer.Serialize(doctor, _jsonOptions);
            StringContent content = new StringContent(doctorJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "doctor", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Doctor>(json, _jsonOptions)!;
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

            IEnumerable<Doctor> doctors = JsonSerializer.Deserialize<IEnumerable<Doctor>>(responseBody, _jsonOptions);

            return doctors;
        }

        public async Task<Doctor> GetByIdAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();
                HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"doctor/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"Doctor with ID {id} was not found.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to get doctor. Status code: {response.StatusCode}. Error: {errorContent}");
                }

                string responseBody = await response.Content.ReadAsStringAsync();

                try
                {
                    Doctor doctor = JsonSerializer.Deserialize<Doctor>(responseBody, _jsonOptions);

                    if (doctor == null)
                    {
                        throw new Exception("Failed to deserialize doctor data");
                    }

                    return doctor;
                }
                catch (JsonException ex)
                {
                    throw new Exception($"Failed to parse doctor data. Response: {responseBody}", ex);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting doctor with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Doctor>> GetByDepartmentIdAsync(int departmentId)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"doctor/department/{departmentId}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            IEnumerable<Doctor> doctors = JsonSerializer.Deserialize<IEnumerable<Doctor>>(responseBody, _jsonOptions);

            return doctors;
        }

        public async Task<bool> UpdateAsync(Doctor doctor)
        {
            AddAuthorizationHeader();
            string doctorJson = JsonSerializer.Serialize(doctor, _jsonOptions);
            StringContent content = new StringContent(doctorJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"doctor/{doctor.Id}", content);
            response.EnsureSuccessStatusCode();

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            return response.IsSuccessStatusCode;
        }
    }
}
