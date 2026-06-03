using JumpDayAPI.Interfaces;
using JumpDayAPI.Models;
using System.Text.Json;

namespace JumpDayAPI.Infrastructure.Weather.Providers
{
    public class OpenMeteoProvider : IWeatherProvider
    {
        private readonly HttpClient _http;

        public OpenMeteoProvider(HttpClient http) => _http = http;

        public async Task<WeatherData?> GetForecastAsync(double lat, double lon)
        {
            string url =
                $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&hourly=temperature_2m,wind_speed_10m,wind_gusts_10m,wind_direction_10m,cloudcover,visibility,precipitation_probability";

            var result = await _http.GetFromJsonAsync<JsonElement>(url);

            if (!result.TryGetProperty("hourly", out var hourly))
                return null;

            return new WeatherData
            {
                Timestamp = DateTime.UtcNow,
                TemperatureC = hourly.GetProperty("temperature_2m")[0].GetDouble(),
                WindSpeedKts = hourly.GetProperty("wind_speed_10m")[0].GetDouble() * 0.539957,
                WindGustKts = hourly.GetProperty("wind_gusts_10m")[0].GetDouble() * 0.539957,
                WindDirectionDeg = hourly.GetProperty("wind_direction_10m")[0].GetDouble(),
                CloudCoverPercent = hourly.GetProperty("cloudcover")[0].GetDouble(),
                VisibilityKm = hourly.GetProperty("visibility")[0].GetDouble() / 1000.0,
                PrecipProbability = hourly.GetProperty("precipitation_probability")[0].GetDouble(),
                CloudCeilingFt = 0, // Open-Meteo doesn’t provide ceiling
                ProviderName = "OpenMeteo"
            };
        }
    }
}
