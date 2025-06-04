using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HMS.Shared.Proxies.Implementations
{
    class LoggerProxy : ILogRepository
    {
        private readonly HttpClient _http_client;
        private static readonly string s_base_api_url = Config._base_api_url;
        private readonly JsonSerializerOptions _json_options;
        private readonly string _token;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerProxy"/> class.
        /// </summary>
        public LoggerProxy(HttpClient http_client, string token)
        {
            this._http_client = http_client;

            this._json_options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            this._token = token;
        }

        private void AddAuthorizationHeader()
        {
            _http_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._token);
        }

        /// <summary>
        /// Gets all logs.
        /// </summary>
        /// <returns>A list of all logs.</returns>
        public async Task<IEnumerable<Log>> GetAllAsync()
        {
            try
            {
                AddAuthorizationHeader();

                HttpResponseMessage response = await this._http_client.GetAsync(s_base_api_url + "log");
                response.EnsureSuccessStatusCode();

                IEnumerable<Log> logs = await response.Content.ReadFromJsonAsync<List<Log>>(this._json_options);
                return logs;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                return new List<Log>();
            }
        }

        /// <summary>
        /// Gets a log by its unique identifier.
        /// </summary>
        /// <param name="id">The id of the log.</param>
        /// <returns>The log with the given id.</returns>
        public async Task<Log?> GetByIdAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();

                HttpResponseMessage response = await this._http_client.GetAsync(s_base_api_url + $"log/{id}");
                response.EnsureSuccessStatusCode();

                Log log = await response.Content.ReadFromJsonAsync<Log>(this._json_options);
                return log;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                throw new Exception($"Log with ID {id} not found.");
            }
        }

        /// <summary>
        /// Adds a new log to the system.
        /// </summary>
        /// <param name="log">The log to be added.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task<Log> AddAsync(Log log)
        {
            try
            {
                AddAuthorizationHeader();

                LogDto log_data = new LogDto
                {
                    Id = log.Id,
                    UserId = log.UserId,
                    Action = log.Action,
                    CreatedAt = log.CreatedAt,
                };

                HttpResponseMessage response = await this._http_client.PostAsJsonAsync(s_base_api_url + "log", log_data);
                response.EnsureSuccessStatusCode();

                Log createdLog = await response.Content.ReadFromJsonAsync<Log>(this._json_options);
                return createdLog;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Log log)
        {
            try
            {
                AddAuthorizationHeader();

                LogDto log_data = new LogDto
                {
                    Id = log.Id,
                    UserId = log.UserId,
                    Action = log.Action,
                    CreatedAt = log.CreatedAt,
                };

                HttpResponseMessage response = await this._http_client.PutAsJsonAsync(s_base_api_url + $"log/{log.Id}", log_data);
                response.EnsureSuccessStatusCode();

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine("Bad Request: The log data is invalid.");
                    return false;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Unexpected response status code.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deletes a log by its unique identifier.
        /// </summary>
        /// <param name="id">The id of the log</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();

                HttpResponseMessage response = await this._http_client.DeleteAsync(s_base_api_url + $"log/{id}");
                response.EnsureSuccessStatusCode();

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Log with ID {id} not found.");
                    return false;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Unexpected response status code.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                throw;
            }
        }
    }
}
