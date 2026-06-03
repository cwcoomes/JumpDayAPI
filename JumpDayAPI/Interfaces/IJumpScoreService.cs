using JumpDayAPI.Models;

namespace JumpDayAPI.Interfaces
{
    public interface IJumpScoreService
    {
        Task<JumpScoreResult> GetJumpScoreAsync(double lat, double lon, UserWeatherPreferences prefs);
    }
}
