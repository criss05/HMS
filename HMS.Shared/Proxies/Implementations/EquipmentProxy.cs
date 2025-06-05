using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;

namespace HMS.Shared.Proxies.Implementations;

public class EquipmentProxy : IEquipmentRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = Config._base_api_url;
    private readonly string _token;

    public EquipmentProxy(HttpClient httpClient, string token)
    {
        this._httpClient = httpClient;
        this._token = token;
    }

    public EquipmentProxy(string token)
    {
        this._httpClient = new HttpClient { BaseAddress = new Uri(this._baseUrl) };
        this._token = token;
    }

    private void AddAuthorizationHeader()
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._token);
    }

    public async Task<List<Equipment>> GetAllAsync()
    {
        AddAuthorizationHeader();
        HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "equipment");
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();

        List<Equipment> equipments = JsonSerializer.Deserialize<List<Equipment>>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
        });

        return equipments;
    }

    public async Task<Equipment?> GetByIdAsync(int id)
    {
        AddAuthorizationHeader();
        HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"equipment/{id}");
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();

        Equipment equipment= JsonSerializer.Deserialize<Equipment>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
        });

        return equipment;
    }

    public async Task AddAsync(Equipment equipment)
    {
        AddAuthorizationHeader();

        var dto = ToDto(equipment);
        string equipmentJson = JsonSerializer.Serialize(dto, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
        });
        StringContent content = new StringContent(equipmentJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "equipment", content);
        response.EnsureSuccessStatusCode();

    }

    public async Task UpdateAsync(Equipment equipment)
    {
        AddAuthorizationHeader();
        string equipmentJson = JsonSerializer.Serialize(ToDto(equipment), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
        });
        StringContent content = new StringContent(equipmentJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"equipment/{equipment.Id}", content);
        response.EnsureSuccessStatusCode();

    }

    public async Task DeleteAsync(int id)
    {
        AddAuthorizationHeader();
        HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"equipment/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        AddAuthorizationHeader();

        HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"equipment/{id}");
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();

        Equipment equipment = JsonSerializer.Deserialize<Equipment>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            } // without this, the enum values will not match
        });

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return false;

        return response.IsSuccessStatusCode;

    }

    private EquipmentDto ToDto(Equipment equipment)
    {
        return new EquipmentDto
        {
            Id = equipment.Id,
            Name = equipment.Name,
            RoomIds = equipment.Rooms.Select(r => r.Id).ToList(),
            Specification = equipment.Specification,
            Stock = equipment.Stock,
            Type = equipment.Type
        };
    }
}