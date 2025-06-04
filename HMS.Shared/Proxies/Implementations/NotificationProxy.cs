using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HMS.Shared.Proxies.Implementations
{
    public class NotificationProxy : INotificationRepository
    {
        /// <summary>
        /// The HTTP client used for sending requests to the Web API.
        /// </summary>

        private readonly HttpClient _http_client;
        private static readonly string s_base_api_url = Config._base_api_url;
        private readonly string _token;

        public NotificationProxy(HttpClient httpClient, string token)
        {
            this._http_client = httpClient;
            _token = token;
        }
        private void AddAuthorizationHeader()
        {
            this._http_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._token);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<NotificationDto>> GetAllAsync()
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await this._http_client.GetAsync(s_base_api_url + "notification");
            response.EnsureSuccessStatusCode();

            String json = await response.Content.ReadAsStringAsync();
            List<NotificationDto> notifications = JsonSerializer.Deserialize<List<NotificationDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return notifications ?? new List<NotificationDto>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int user_id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await this._http_client.GetAsync(s_base_api_url + $"notification/user/{user_id}");
            response.EnsureSuccessStatusCode();

            String json = await response.Content.ReadAsStringAsync();
            IEnumerable<Notification> notifications = JsonSerializer.Deserialize<IEnumerable<Notification>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return notifications ?? new List<Notification>();
        }

        /// <inheritdoc/>
        public async Task<Notification> GetByIdAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await this._http_client.GetAsync(s_base_api_url + $"notification/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"Notification with ID {id} was not found.");
            }

            response.EnsureSuccessStatusCode();

            string _json = await response.Content.ReadAsStringAsync();

            Notification _notification = JsonSerializer.Deserialize<Notification>(_json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return _notification ?? throw new Exception("Failed to deserialize notification.");
        }

        /// <inheritdoc/>
        public async Task<Notification> AddAsync(Notification notification)
        {
            AddAuthorizationHeader();
            StringContent jsonContent = new StringContent(
                JsonSerializer.Serialize(notification),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await this._http_client.PostAsync(s_base_api_url + "notification", jsonContent);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            Notification createdNotification = JsonSerializer.Deserialize<Notification>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return createdNotification;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await this._http_client.DeleteAsync(s_base_api_url + $"notification/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"Notification with ID {id} was not found.");
            }

            response.EnsureSuccessStatusCode();

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true; // Deletion successful
            }
            return false;
        }

        public async Task<bool> UpdateAsync(Notification notification)
        {
            AddAuthorizationHeader();
            StringContent jsonContent = new StringContent(
                JsonSerializer.Serialize(notification),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await this._http_client.PostAsync(s_base_api_url + $"notification/{notification.Id}", jsonContent);
            response.EnsureSuccessStatusCode();

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true; // Update successful
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"Notification with ID {notification.Id} was not found.");
            }
            else
            {
                return false; // Update failed for some reason

            }
        }
    }
}
