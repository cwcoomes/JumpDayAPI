using JumpDayAPI.Interfaces;
using JumpDayAPI.Models;

namespace JumpDayAPI.Endpoints
{
    public static class JumpScoreEndpoints
    {
        public static void AddJumpScoreEndpoints(this WebApplication app)
        {
            // Forecast endpoint
            app.MapPost("/api/jump-score", async (
                double lat,
                double lon,
                UserWeatherPreferences prefs,
                IJumpScoreService service) =>
            {
                var score = await service.GetJumpScoreAsync(lat, lon, prefs);
                return Results.Ok(score);
            });
        }

    }
}
