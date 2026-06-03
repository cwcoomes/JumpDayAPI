namespace JumpDayAPI.Models
{
    public class UserWeatherPreferences
    {
        public double WindWeight { get; set; } = 0.4;
        public double CloudWeight { get; set; } = 0.2;
        public double TempWeight { get; set; } = 0.1;
        public double VisibilityWeight { get; set; } = 0.2;
        public double PrecipWeight { get; set; } = 0.1;
    }
}
