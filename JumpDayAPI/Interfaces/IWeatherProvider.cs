using JumpDayAPI.Models;

namespace JumpDayAPI.Interfaces
{
    public interface IWeatherProvider
    {
        Task<WeatherData?> GetForecastAsync(double lat, double lon);
    }
}
