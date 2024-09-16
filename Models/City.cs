namespace WeatherServiceAPI.Models
{
    public class City
    {
        public string name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string country { get; set; }
        public string state { get; set; }
    }
    public class CityList<L>
    {
        /// <summary>
        /// List of cities 
        /// </summary>
        public List<L> Cities { get; set; }
    }
}
