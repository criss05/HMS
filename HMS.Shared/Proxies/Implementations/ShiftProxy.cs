using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using HMS.Shared.DTOs;

namespace HMS.Shared.Proxies.Implementations;

public class ShiftProxy : IShiftRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = Config._base_api_url;
    private readonly string _token;

    public ShiftProxy(HttpClient httpClient, string token)
    {
        this._httpClient = httpClient;
        this._token = token;
    }

    public ShiftProxy(string token)
    {
        this._httpClient = new HttpClient { BaseAddress = new Uri(this._baseUrl) };
        this._token = token;
    }

    private void AddAuthorizationHeader()
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._token);
    }

    public async Task<IEnumerable<Shift>> GetAllAsync()
    {
        AddAuthorizationHeader();
        HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "shift");
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();

        IEnumerable<Shift> shifts = JsonSerializer.Deserialize<IEnumerable<Shift>>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
        });

        return shifts;
    }

    public async Task<Shift?> GetByIdAsync(int id)
    {
        AddAuthorizationHeader();
        HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"shift/{id}");
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();

        Shift shift= JsonSerializer.Deserialize<Shift>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
        });

        return shift;
    }

    public async Task<Shift> AddAsync(Shift shift)
    {
        AddAuthorizationHeader();
        string shiftJson = JsonSerializer.Serialize(toDto(shift), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
        });
        StringContent content = new StringContent(shiftJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "shift", content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Shift>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
        })!;
    }

    public async Task<bool> UpdateAsync(Shift shift)
    {
        AddAuthorizationHeader();
        string shiftJson = JsonSerializer.Serialize(toDto(shift), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
        });
        StringContent content = new StringContent(shiftJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"shift/{shift.Id}", content);
        response.EnsureSuccessStatusCode();

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return false;

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        AddAuthorizationHeader();
        HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"shift/{id}");
        response.EnsureSuccessStatusCode();

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return false;

        return response.IsSuccessStatusCode;
    }

    private ShiftDto toDto(Shift shift)
    {
        return new ShiftDto
        {
            Date = shift.Date,
            EndTime = shift.EndTime,
            StartTime = shift.StartTime,
            DoctorIds = shift.Schedules.Select(s => s.DoctorId).ToList()
        };
    }
}