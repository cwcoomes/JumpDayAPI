using JumpDayAPI.Models;
using System.Text.Json;

namespace JumpDayAPI.Services;

public class DropZoneService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private List<DropZoneModel> _dropZones = new();
    private readonly SemaphoreSlim _refreshLock = new(1, 1);

    public IReadOnlyList<DropZoneModel> DropZones => _dropZones.AsReadOnly();

    public DropZoneService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task RefreshAsync()
    {
        await _refreshLock.WaitAsync();
        try
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "DZName", "" },
                { "City", "" },
                { "Region", "" },
                { "Country", "US" },
                { "State", "" }
            });

            var response = await _httpClient.PostAsync("/api/DZList", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            _dropZones = JsonSerializer.Deserialize<List<DropZoneModel>>(json, _jsonOptions) ?? new();
        }
        finally
        {
            _refreshLock.Release();
        }
    }
}
