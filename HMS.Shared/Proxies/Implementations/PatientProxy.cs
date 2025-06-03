using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using HMS.Shared.DTOs.Patient;

namespace HMS.Shared.Proxies.Implementations
{
    public class PatientProxy : IPatientRepository
    {
        private readonly HttpClient _http_client;
        private readonly string s_base_api_url = Config._base_api_url;
        private readonly string _token;

        public PatientProxy(HttpClient http_client, string token)
        {
            this._http_client = http_client;
            this._token = token;
        }

        private void AddAuthorizationHeader()
        {
            this._http_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._token);
        }

        public async Task<IEnumerable<PatientDto>> GetAllAsync()
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await this._http_client.GetAsync(this.s_base_api_url + "patient");
            response.EnsureSuccessStatusCode();

            string response_body = await response.Content.ReadAsStringAsync();

            IEnumerable<PatientDto> patients = JsonSerializer.Deserialize<IEnumerable<PatientDto>>(response_body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return patients;
        }

        public async Task<PatientDto> GetByIdAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await this._http_client.GetAsync($"{this.s_base_api_url}patient/{id}");
            response.EnsureSuccessStatusCode();

            string response_body = await response.Content.ReadAsStringAsync();

            PatientDto patient = JsonSerializer.Deserialize<PatientDto>(response_body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (patient == null)
                throw new Exception($"No patient found with user id {id}");

            return patient;
        }

        public async Task<PatientDto> AddAsync(Patient patient)
        {
            AddAuthorizationHeader();
            PatientCreateDto patient_send = new PatientCreateDto
            {
                Email = patient.Email,
                Password = patient.Password,
                Name = patient.Name,
                CNP = patient.CNP,
                PhoneNumber = patient.PhoneNumber,
                Role = patient.Role.ToString(),
                BloodType = patient.BloodType.ToString(),
                EmergencyContact = patient.EmergencyContact,
                Allergies = patient.Allergies,
                Weight = patient.Weight,
                Height = patient.Height,
                BirthDate = patient.BirthDate,
                Address = patient.Address
            };

            StringContent jsonContent = new StringContent(
                JsonSerializer.Serialize(patient_send),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await this._http_client.PostAsync(this.s_base_api_url + "patient", jsonContent);
            response.EnsureSuccessStatusCode();

            string response_body = await response.Content.ReadAsStringAsync();

            PatientDto patient_response = JsonSerializer.Deserialize<PatientDto>(response_body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return patient_response;
        }

        public async Task<bool> UpdateAsync(Patient patient)
        {
            AddAuthorizationHeader();
            try
            {
                PatientUpdateDto patient_send = new PatientUpdateDto
                {
                    Email = patient.Email,
                    Password = patient.Password,
                    Name = patient.Name,
                    CNP = patient.CNP,
                    PhoneNumber = patient.PhoneNumber,
                    Role = patient.Role.ToString(),
                    BloodType = patient.BloodType.ToString(),
                    EmergencyContact = patient.EmergencyContact,
                    Allergies = patient.Allergies,
                    Weight = patient.Weight,
                    Height = patient.Height,
                    BirthDate = patient.BirthDate,
                    Address = patient.Address
                };

                StringContent jsonContent = new StringContent(
                    JsonSerializer.Serialize(patient_send),
                    Encoding.UTF8,
                    "application/json");

                HttpResponseMessage patient_response = await this._http_client.PutAsync($"{this.s_base_api_url}patient/{patient.Id}", jsonContent);

                patient_response.EnsureSuccessStatusCode();

                if (patient_response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                return patient_response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating patient data: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await this._http_client.DeleteAsync($"{this.s_base_api_url}patient/{id}");
            response.EnsureSuccessStatusCode();

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            return response.IsSuccessStatusCode;
        }
    }
}
