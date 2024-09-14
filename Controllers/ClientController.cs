using Microsoft.AspNetCore.Mvc;
using WeatherServiceAPI.Controllers.Models;
using Microsoft.Extensions.Configuration;
using WeatherServiceAPI.Utils;
using WeatherServiceAPI.Models;

namespace WeatherServiceAPI.Controllers
{
    public class ClientController : ControllerBase
    {
        private readonly Operations _operations;
        public ClientController(IConfiguration configuration) {
            _operations = new Operations(configuration["URLWeather"], configuration["Parameters"]);            

        }
        [HttpGet]
        [Route("consumir")]
        public async Task<WeatherData> consumirAsync([FromQuery] float latitude, float longitude)
        {
            Coordinates coordinates = new Coordinates();
            WeatherData weatherData = new WeatherData();
            coordinates.latitude = latitude;
            coordinates.longitude = longitude;
              
                weatherData = await _operations.ExtractDataFromMongo(coordinates.latitude, coordinates.longitude);
                return weatherData;

            

        }
    }
}
