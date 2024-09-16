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
            _operations = new Operations(
                configuration["URLWeather"], 
                configuration["Parameters"], 
                configuration["DBConnection"],
                configuration["DBname"],
                configuration["CollectionName"],
                configuration["URLCity"], 
                configuration["ApiKey"]         
            );            

        }
        [HttpGet]
        [Route("ForecastByCoordinates")]
        public async Task<IActionResult> consumirAsync([FromQuery] double latitude, double longitude)
        {
            Coordinates coordinates = new ();
            GeneralResponse generalResponse = new ();
            try
            {
                coordinates.latitude = latitude;
                coordinates.longitude = longitude;
                generalResponse = await _operations.ExtractData(coordinates);
                return Ok(generalResponse);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("ForecastByCity")]
        public async Task<IActionResult> consumirCiudad([FromQuery] string ciudad)
        {
            try
            {
                var response = await _operations.ConsumirCiudad(ciudad);
                return Ok(response);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
