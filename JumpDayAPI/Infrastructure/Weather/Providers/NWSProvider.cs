using JumpDayAPI.Interfaces;
using JumpDayAPI.Models;
using System.Text.Json;

namespace JumpDayAPI.Infrastructure.Weather.Providers
{
    public class NWSProvider : IWeatherProvider
    {
        private readonly HttpClient _http;

        public NWSProvider(HttpClient http)
        {
            _http = http;
            _http.DefaultRequestHeaders.Add("User-Agent", "JumpDayAPI/1.0");
        }

        public async Task<WeatherData?> GetForecastAsync(double lat, double lon)
        {
            try
            {
                // 1. Get gridpoint info
                var metaUrl = $"https://api.weather.gov/points/{lat},{lon}";
                var metaJson = await _http.GetFromJsonAsync<JsonElement>(metaUrl);

                if (!metaJson.TryGetProperty("properties", out var props) ||
                    !props.TryGetProperty("forecastHourly", out var hourlyUrlJson))
                    return null;

                var hourlyUrl = hourlyUrlJson.GetString();
                if (hourlyUrl == null) return null;

                // 2. Fetch hourly forecast
                var hourlyJson = await _http.GetFromJsonAsync<JsonElement>(hourlyUrl);

                if (!hourlyJson.TryGetProperty("properties", out var hourlyProps) ||
                    !hourlyProps.TryGetProperty("periods", out var periods))
                    return null;

                var period = periods[0]; // current hour

                double windSpeedKts = ParseWindSpeed(period.GetProperty("windSpeed").GetString());
                double windGustKts = ParseWindSpeed(period.GetProperty("windGust").GetString());

                return new WeatherData
                {
                    Timestamp = DateTime.UtcNow,
                    TemperatureC = FahrenheitToC(period.GetProperty("temperature").GetDouble()),
                    WindSpeedKts = windSpeedKts,
                    WindGustKts = windGustKts,
                    WindDirectionDeg = ParseWindDirection(period.GetProperty("windDirection").GetString()),
                    CloudCoverPercent = period.TryGetProperty("cloudCover", out var cc) ? cc.GetDouble() : 0,
                    VisibilityKm = 10, // NWS doesn't provide visibility in this endpoint easily
                    PrecipProbability = period.GetProperty("probabilityOfPrecipitation")
                                         .GetProperty("value").GetDouble(),
                    CloudCeilingFt = 0, // only available in aviation endpoint
                    ProviderName = "NWS"
                };
            }
            catch
            {
                return null; // fail softly
            }
        }

        // Helpers

        private double FahrenheitToC(double f) => (f - 32) * 5 / 9;

        private double ParseWindSpeed(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;

            var num = new string(text.TakeWhile(char.IsDigit).ToArray());
            if (!double.TryParse(num, out double mph)) return 0;

            return mph * 0.868976; // mph -> knots
        }

        private double ParseWindDirection(string? dir)
        {
            if (string.IsNullOrWhiteSpace(dir)) return 0;

            // NWS uses compass directions like "NW"
            return dir.ToUpper() switch
            {
                "N" => 0,
                "NE" => 45,
                "E" => 90,
                "SE" => 135,
                "S" => 180,
                "SW" => 225,
                "W" => 270,
                "NW" => 315,
                _ => 0
            };
        }
    }

}
