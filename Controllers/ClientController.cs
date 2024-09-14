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
            _operations = new Operations(configuration["URLWeather"]);            

        }
        [HttpGet]
        [Route("consumir")]
        public async Task<string> consumirAsync([FromQuery] float latitude, float longitude)
        {
            Coordinates coordinates = new Coordinates();
            coordinates.latitude = latitude;
            coordinates.longitude = longitude;
            try
            {    
                var result = await _operations.ExtractData(coordinates.latitude, coordinates.longitude);
                return result;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
