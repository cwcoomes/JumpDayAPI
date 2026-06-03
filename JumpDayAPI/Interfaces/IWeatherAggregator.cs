using JumpDayAPI.Models;

namespace JumpDayAPI.Interfaces
{
    public interface IWeatherAggregator
    {
        Task<AggregatedWeather> GetAggregatedWeatherAsync(double lat, double lon);
    }
}
