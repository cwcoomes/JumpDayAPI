using JumpDayAPI.Interfaces;
using JumpDayAPI.Models;

namespace JumpDayAPI.Infrastructure.Weather
{
    public class WeatherAggregator : IWeatherAggregator
    {
        private readonly IEnumerable<IWeatherProvider> _providers;

        public WeatherAggregator(IEnumerable<IWeatherProvider> providers)
        {
            _providers = providers;
        }

        public async Task<AggregatedWeather> GetAggregatedWeatherAsync(double lat, double lon)
        {
            IEnumerable<Task<WeatherData?>> tasks = _providers.Select(p => p.GetForecastAsync(lat, lon));
            List<WeatherData?> results = (await Task.WhenAll(tasks))
                .Where(r => r != null)
                .ToList()!;

            if (!results.Any())
                throw new Exception("No weather providers returned data.");

            // If only one provider works, skip averaging entirely.
            if (results.Count == 1)
                return SingleProviderResult(results[0]!);

            // Weighted average (all providers currently equal weight)
            AggregatedWeather agg = new AggregatedWeather
            {
                Timestamp = DateTime.UtcNow,
                TemperatureC = results.Average(r => r.TemperatureC),
                WindSpeedKts = results.Average(r => r.WindSpeedKts),
                WindGustKts = results.Average(r => r.WindGustKts),
                WindDirectionDeg = CircularAverage(results.Select(r => r.WindDirectionDeg)),
                CloudCoverPercent = results.Average(r => r.CloudCoverPercent),
                VisibilityKm = results.Average(r => r.VisibilityKm),
                PrecipProbability = results.Average(r => r.PrecipProbability),
                CloudCeilingFt = results.Average(r => r.CloudCeilingFt),
                ProviderBreakdown = results
            };

            return agg;
        }

        private AggregatedWeather SingleProviderResult(WeatherData data)
        {
            return new AggregatedWeather
            {
                Timestamp = data.Timestamp,
                TemperatureC = data.TemperatureC,
                WindSpeedKts = data.WindSpeedKts,
                WindGustKts = data.WindGustKts,
                WindDirectionDeg = data.WindDirectionDeg,
                CloudCoverPercent = data.CloudCoverPercent,
                VisibilityKm = data.VisibilityKm,
                PrecipProbability = data.PrecipProbability,
                CloudCeilingFt = data.CloudCeilingFt,
                ProviderBreakdown = new List<WeatherData> { data }
            };
        }

        // Handles wind direction averaging properly
        private double CircularAverage(IEnumerable<double> angles)
        {
            double sin = angles.Select(a => Math.Sin(a * Math.PI / 180)).Average();
            double cos = angles.Select(a => Math.Cos(a * Math.PI / 180)).Average();
            double avg = Math.Atan2(sin, cos) * (180 / Math.PI);
            return (avg + 360) % 360;
        }
    }
}
