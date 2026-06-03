namespace JumpDayAPI.Models
{
    public class AggregatedWeather
    {
        public DateTime Timestamp { get; set; }
        public double TemperatureC { get; set; }
        public double WindSpeedKts { get; set; }
        public double WindGustKts { get; set; }
        public double WindDirectionDeg { get; set; }
        public double CloudCoverPercent { get; set; }
        public double VisibilityKm { get; set; }
        public double PrecipProbability { get; set; }
        public double CloudCeilingFt { get; set; }
        public List<WeatherData> ProviderBreakdown { get; set; } = new();
    }
}
