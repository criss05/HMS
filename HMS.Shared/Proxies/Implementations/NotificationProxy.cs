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

        public NotificationProxy(HttpClient _http_client, string token)
        {
            this._http_client = _http_client;
            _token = token;
        }
        private void AddAuthorizationHeader()
        {
            this._http_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._token);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await this._http_client.GetAsync(s_base_api_url + "notification");
            response.EnsureSuccessStatusCode();

            String json = await response.Content.ReadAsStringAsync();
            List<Notification> notifications = JsonSerializer.Deserialize<List<Notification>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return notifications ?? new List<Notification>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int user_id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await this._http_client.GetAsync(s_base_api_url + $"notification/user/{user_id}");
            response.EnsureSuccessStatusCode();

            String json = await response.Content.ReadAsStringAsync();
            List<Notification> notifications = JsonSerializer.Deserialize<List<Notification>>(json, new JsonSerializerOptions
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
        public async Task AddAsync(Notification notification)
        {
            AddAuthorizationHeader();
            StringContent jsonContent = new StringContent(
                JsonSerializer.Serialize(notification),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await this._http_client.PostAsync(s_base_api_url + "notification", jsonContent);
            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await this._http_client.DeleteAsync(s_base_api_url + $"notification/delete/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"Notification with ID {id} was not found.");
            }

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(Notification notification)
        {
            AddAuthorizationHeader();
            StringContent jsonContent = new StringContent(
                JsonSerializer.Serialize(notification),
                Encoding.UTF8,
                "application/json");
    
            HttpResponseMessage response = await this._http_client.PostAsync(s_base_api_url + $"notification/update/{notification.Id}", jsonContent);
            response.EnsureSuccessStatusCode();
        }
    }
}
