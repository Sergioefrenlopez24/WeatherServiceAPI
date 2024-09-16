using WeatherServiceAPI.Controllers.Models;
using WeatherServiceAPI.Models;
using WeatherServiceAPI.Helper;
using Newtonsoft.Json;

namespace WeatherServiceAPI.Utils
{
    public class ExtractDataFromExternalAPI
    {
        private readonly string _baseUrl;       
        private readonly string _apiKey;
        private readonly string _URLCity;
        private readonly HttpHelper _httpHelper;
        public ExtractDataFromExternalAPI(string baseUrl, string apiKey, string uRLCity)
        {
            _baseUrl = baseUrl;
            _httpHelper = new HttpHelper();
            _apiKey = apiKey;
            _URLCity = uRLCity;
        }
        public async Task<WeatherData> ExtractData(Coordinates coordinates)
        {
            WeatherData weatherData = new();
            var response = await _httpHelper.HttpRequest(string.Format(_baseUrl, coordinates.latitude, coordinates.longitude), "");
            weatherData = JsonConvert.DeserializeObject<WeatherData>(response);
            return weatherData;
        }
        public async Task<List<City>> ObtainCities(string cities)
        {
            CityList<City> city = new();
            var response = await _httpHelper.HttpRequest(string.Format(_URLCity,cities),_apiKey);
            city.Cities = JsonConvert.DeserializeObject<List<City>>(response);
            return city.Cities;
        }
    }
}
