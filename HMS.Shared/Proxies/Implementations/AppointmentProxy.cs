using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Linq;

namespace HMS.Shared.Proxies.Implementations
{
    public class AppointmentProxy : IAppointmentRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = Config._base_api_url;
        private readonly string _token;
        private readonly JsonSerializerOptions _jsonOptions;

        public AppointmentProxy(HttpClient httpClient, string token)
        {
            _httpClient = httpClient;
            _token = token;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
        }

        public AppointmentProxy(string token)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
            _token = token;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
        }

        private void AddAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
        }

        public async Task<AppointmentDto> AddAsync(AppointmentDto appointment)
        {
            try
            {
                AddAuthorizationHeader();
                string appointmentJson = JsonSerializer.Serialize(appointment, _jsonOptions);
                StringContent content = new StringContent(appointmentJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "appointment", content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AppointmentDto>(responseBody, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding appointment: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();
                HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"appointment/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting appointment: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            try
            {
                AddAuthorizationHeader();
                HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "appointment");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Try to deserialize as a reference-tracked array
                try 
                {
                    var appointments = new List<AppointmentDto>();
                    var jsonDoc = JsonDocument.Parse(responseBody);

                    // Function to parse appointment from JsonElement
                    void ParseAppointment(JsonElement element)
                    {
                        try
                        {
                            // Handle reference
                            if (element.TryGetProperty("$ref", out var refElement))
                            {
                                // Skip already processed references
                                return;
                            }

                            var appointment = new AppointmentDto();

                            if (element.TryGetProperty("id", out var idElement))
                                appointment.Id = idElement.GetInt32();

                            if (element.TryGetProperty("patientId", out var patientIdElement))
                                appointment.PatientId = patientIdElement.GetInt32();

                            if (element.TryGetProperty("doctorId", out var doctorIdElement))
                                appointment.DoctorId = doctorIdElement.GetInt32();

                            if (element.TryGetProperty("procedureId", out var procedureIdElement))
                                appointment.ProcedureId = procedureIdElement.GetInt32();

                            if (element.TryGetProperty("roomId", out var roomIdElement))
                                appointment.RoomId = roomIdElement.GetInt32();

                            if (element.TryGetProperty("dateTime", out var dateTimeElement))
                                appointment.DateTime = dateTimeElement.GetDateTime();

                            if (appointment.Id.HasValue && appointment.DoctorId != 0 && appointment.PatientId != 0)
                            {
                                // Only add if not already in the list
                                if (!appointments.Any(a => a.Id == appointment.Id))
                                {
                                    appointments.Add(appointment);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error parsing appointment: {ex.Message}");
                        }
                    }

                    // Process root values
                    if (jsonDoc.RootElement.TryGetProperty("$values", out var valuesElement))
                    {
                        foreach (var element in valuesElement.EnumerateArray())
                        {
                            ParseAppointment(element);
                        }
                    }

                    // Also check doctor's appointments if present
                    if (jsonDoc.RootElement.TryGetProperty("$values", out var rootValues))
                    {
                        foreach (var element in rootValues.EnumerateArray())
                        {
                            if (element.TryGetProperty("doctor", out var doctorElement) &&
                                doctorElement.TryGetProperty("appointments", out var doctorAppointments) &&
                                doctorAppointments.TryGetProperty("$values", out var appointmentsValues))
                            {
                                foreach (var appointmentElement in appointmentsValues.EnumerateArray())
                                {
                                    ParseAppointment(appointmentElement);
                                }
                            }
                        }
                    }

                    return appointments;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing reference-tracked response: {ex.Message}");
                }

                // Fallback to direct deserialization
                return JsonSerializer.Deserialize<List<AppointmentDto>>(responseBody, _jsonOptions) ?? new List<AppointmentDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all appointments: {ex.Message}");
                throw;
            }
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();
                HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"appointment/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AppointmentDto>(responseBody, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting appointment by ID: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(AppointmentDto appointment)
        {
            try
            {
                AddAuthorizationHeader();
                string appointmentJson = JsonSerializer.Serialize(appointment, _jsonOptions);
                StringContent content = new StringContent(appointmentJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"appointment/{appointment.Id}", content);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating appointment: {ex.Message}");
                throw;
            }
        }
    }
} 