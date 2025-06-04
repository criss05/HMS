using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HMS.Shared.Proxies.Implementations
{
    public class MedicalRecordProxy : IMedicalRecordRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = Config._base_api_url;
        private readonly string _token;
        private readonly JsonSerializerOptions _jsonOptions;

        public MedicalRecordProxy(HttpClient httpClient, string token)
        {
            this._httpClient = httpClient;
            this._token = token;
            this._jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        private void AddAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._token);
        }

        public async Task<MedicalRecord> AddAsync(MedicalRecord medicalRecord)
        {
            AddAuthorizationHeader();
            string recordJson = JsonSerializer.Serialize(medicalRecord, _jsonOptions);
            StringContent content = new StringContent(recordJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "medicalrecord", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<MedicalRecord>(json, _jsonOptions)!;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"medicalrecord/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return true;
        }

        private class MedicalRecordResponse
        {
            public IEnumerable<MedicalRecord> Records { get; set; } = new List<MedicalRecord>();
        }

        public async Task<IEnumerable<MedicalRecord>> GetAllAsync()
        {
            try
            {
                AddAuthorizationHeader();
                HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "medicalrecord");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                try
                {
                    var result = JsonSerializer.Deserialize<MedicalRecordResponse>(responseBody, _jsonOptions);
                    if (result == null || result.Records == null)
                    {
                        throw new Exception("Failed to deserialize medical records data");
                    }
                    return result.Records;
                }
                catch (JsonException ex)
                {
                    throw new Exception($"Failed to parse medical records data. Response: {responseBody}", ex);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting medical records: {ex.Message}");
                throw;
            }
        }

        public async Task<MedicalRecord?> GetByIdAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();
                HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"medicalrecord/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                try
                {
                    var record = JsonSerializer.Deserialize<MedicalRecord>(responseBody, _jsonOptions);
                    if (record == null)
                    {
                        throw new Exception("Failed to deserialize medical record data");
                    }
                    return record;
                }
                catch (JsonException ex)
                {
                    throw new Exception($"Failed to parse medical record data. Response: {responseBody}", ex);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting medical record with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(MedicalRecord medicalRecord)
        {
            AddAuthorizationHeader();
            string recordJson = JsonSerializer.Serialize(medicalRecord, _jsonOptions);
            StringContent content = new StringContent(recordJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"medicalrecord/{medicalRecord.Id}", content);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return true;
        }
    }
} 