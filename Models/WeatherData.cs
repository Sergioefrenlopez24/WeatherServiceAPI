namespace WeatherServiceAPI.Controllers.Models
{
    public class WeatherData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double GenerationTimeMs { get; set; }
        public int UtcOffsetSeconds { get; set; }
        public string Timezone { get; set; }
        public string TimezoneAbbreviation { get; set; }
        public int Elevation { get; set; }
        public HourlyUnits HourlyUnits { get; set; }
        public HourlyData Hourly { get; set; }
        public DailyUnits DailyUnits { get; set; }
        public DailyData Daily { get; set; }
    }

    public class HourlyUnits
    {
        public string Time { get; set; }
        public string Temperature2m { get; set; }
        public string WindDirection10m { get; set; }
        public string WindSpeed10m { get; set; }
    }

    public class HourlyData
    {
        public List<string> Time { get; set; }
        public List<double> Temperature2m { get; set; }
        public List<int> WindDirection10m { get; set; }
        public List<double> WindSpeed10m { get; set; }
    }

    public class DailyUnits
    {
        public string Time { get; set; }
        public string Sunrise { get; set; }
    }

    public class DailyData
    {
        public List<string> Time { get; set; }
        public List<string> Sunrise { get; set; }
    }
}
