using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using HMS.Shared.DTOs;
using System.Diagnostics;

namespace HMS.Shared.Proxies.Implementations
{
    public class ReviewProxy : IReviewRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = Config._base_api_url;
        private readonly string _token;
        private readonly JsonSerializerOptions _jsonOptions;

        public ReviewProxy(HttpClient httpClient, string token)
        {
            _httpClient = httpClient;
            _token = token;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
        }

        public ReviewProxy(string token)
        {
            this._httpClient = new HttpClient { BaseAddress = new Uri(this._baseUrl) };
            _token = token;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
        }

        private void AddAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}review");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var reviews = JsonSerializer.Deserialize<IEnumerable<Review>>(responseBody, _jsonOptions);
            return reviews ?? new List<Review>();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}review/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Review>(responseBody, _jsonOptions);
        }

        public async Task<Review> AddAsync(Review review)
        {
            var dto = new ReviewDto
            {
                PatientId = review.PatientId,
                DoctorId = review.DoctorId,
                Value = review.Value
            };

            AddAuthorizationHeader();
            string reviewJson = JsonSerializer.Serialize(dto, _jsonOptions);
            StringContent content = new StringContent(reviewJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{_baseUrl}review", content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"StatusCode: {response.StatusCode}");
            Debug.WriteLine($"ResponseBody: {responseBody}");
            return JsonSerializer.Deserialize<Review>(responseBody, _jsonOptions)!;
        }

        public async Task<bool> UpdateAsync(Review review)
        {
            var dto = new ReviewDto
            {
                Id = review.Id,
                PatientId = review.PatientId,
                DoctorId = review.DoctorId,
                Value = review.Value
            };

            AddAuthorizationHeader();
            string reviewJson = JsonSerializer.Serialize(dto, _jsonOptions);
            StringContent content = new StringContent(reviewJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{_baseUrl}review/{review.Id}", content);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_baseUrl}review/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }
    }
}
