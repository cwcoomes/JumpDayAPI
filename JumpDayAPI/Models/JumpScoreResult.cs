namespace JumpDayAPI.Models
{
    public class JumpScoreResult
    {
        public int Score { get; set; }
        public AggregatedWeather Weather { get; set; }
        public Dictionary<string, int> FactorBreakdown { get; set; } = new();
    }
}
