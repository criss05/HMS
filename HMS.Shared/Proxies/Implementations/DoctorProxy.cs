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
    class DoctorProxy : IDoctorRepository
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
            string doctorJson = JsonSerializer.Serialize(doctor);
            StringContent content = new StringContent(doctorJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "api/doctor", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Doctor>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"api/doctor/delete/{id}");
            response.EnsureSuccessStatusCode();

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "api/doctor");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            List<Doctor> doctors = JsonSerializer.Deserialize<List<Doctor>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return doctors;
        }

        public async Task<Doctor> GetByIdAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"api/doctor/{id}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            Doctor doctor = JsonSerializer.Deserialize<Doctor>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return doctor;
        }

        public async Task<List<Doctor>> GetByDepartmentIdAsync(int departmentId)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"api/doctor/doctor/{departmentId}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            List<Doctor> doctors = JsonSerializer.Deserialize<List<Doctor>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return doctors;
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            AddAuthorizationHeader();
            var departments = await GetAllDepartmentsAsync();
            return departments.FirstOrDefault(d => d.Id == id);
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "api/department");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Department>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<bool> UpdateAsync(Doctor doctor)
        {
            AddAuthorizationHeader();
            string doctorJson = JsonSerializer.Serialize(doctor);
            StringContent content = new StringContent(doctorJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"api/doctor/{doctor.Id}", content);
            response.EnsureSuccessStatusCode();

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            return response.IsSuccessStatusCode;
        }

        public async Task UpdateByNameAsync(string name, Doctor doctor)
        {
            AddAuthorizationHeader();
            string doctorJson = JsonSerializer.Serialize(doctor);
            StringContent content = new StringContent(doctorJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"api/doctor/by-name/{Uri.EscapeDataString(name)}", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
