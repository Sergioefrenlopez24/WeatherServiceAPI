using MongoDB.Bson;

namespace WeatherServiceAPI.Controllers.Models
{
    public class WeatherData
    {
        public ObjectId _id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double generationtime_ms { get; set; }
        public int utc_offset_seconds { get; set; }
        public string timezone { get; set; }
        public string timezone_abbreviation { get; set; }
        public int elevation { get; set; }
        public HourlyUnits hourly_units { get; set; }
        public HourlyData hourly { get; set; }
        public DailyUnits daily_units { get; set; }
        public DailyData daily { get; set; }
    }

    public class HourlyUnits
    {
        public string time { get; set; }
        public string temperature_2m { get; set; }
        public string wind_direction_10m { get; set; }
        public string wind_speed_10m { get; set; }
    }

    public class HourlyData
    {
        public List<string> time { get; set; }
        public List<double> temperature_2m { get; set; }
        public List<int> wind_direction_10m { get; set; }
        public List<double> wind_speed_10m { get; set; }
    }

    public class DailyUnits
    {
        public string time { get; set; }
        public string sunrise { get; set; }
    }

    public class DailyData
    {
        public List<string> time { get; set; }
        public List<string> sunrise { get; set; }
    }
}
