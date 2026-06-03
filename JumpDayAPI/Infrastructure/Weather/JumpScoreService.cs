using JumpDayAPI.Interfaces;
using JumpDayAPI.Models;

namespace JumpDayAPI.Infrastructure.Weather
{
    public class JumpScoreService : IJumpScoreService
    {
        private readonly IWeatherAggregator _aggregator;

        public JumpScoreService(IWeatherAggregator aggregator)
        {
            _aggregator = aggregator;
        }

        public async Task<JumpScoreResult> GetJumpScoreAsync(double lat, double lon, UserWeatherPreferences prefs)
        {
            var w = await _aggregator.GetAggregatedWeatherAsync(lat, lon);

            var factors = new Dictionary<string, int>();

            int windScore = ScoreWind(w.WindSpeedKts);
            factors["Wind"] = windScore;

            int cloudScore = ScoreCloud(w.CloudCoverPercent, w.CloudCeilingFt);
            factors["Cloud"] = cloudScore;

            int tempScore = ScoreTemp(w.TemperatureC);
            factors["Temperature"] = tempScore;

            int visScore = ScoreVisibility(w.VisibilityKm);
            factors["Visibility"] = visScore;

            int precipScore = ScorePrecip(w.PrecipProbability);
            factors["Precipitation"] = precipScore;

            double finalScore =
                (windScore * prefs.WindWeight) +
                (cloudScore * prefs.CloudWeight) +
                (tempScore * prefs.TempWeight) +
                (visScore * prefs.VisibilityWeight) +
                (precipScore * prefs.PrecipWeight);

            return new JumpScoreResult
            {
                Score = (int)Math.Round(finalScore),
                Weather = w,
                FactorBreakdown = factors
            };
        }

        private int ScoreWind(double kts)
        {
            if (kts <= 8) return 100;
            if (kts <= 12) return 80;
            if (kts <= 16) return 60;
            if (kts <= 20) return 40;
            return 20;
        }

        private int ScoreCloud(double cover, double ceilingFt)
        {
            if (cover < 20) return 100;
            if (cover < 50) return 80;
            if (cover < 80) return 60;
            return 40;
        }

        private int ScoreTemp(double c)
        {
            if (c >= 15 && c <= 30) return 100;
            return 60;
        }

        private int ScoreVisibility(double km)
        {
            if (km >= 10) return 100;
            if (km >= 5) return 70;
            return 40;
        }

        private int ScorePrecip(double p)
        {
            if (p < 10) return 100;
            if (p < 40) return 60;
            return 20;
        }
    }
}
