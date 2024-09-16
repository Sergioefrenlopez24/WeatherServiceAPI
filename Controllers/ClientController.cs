using Microsoft.AspNetCore.Mvc;
using WeatherServiceAPI.Controllers.Models;
using Microsoft.Extensions.Configuration;
using WeatherServiceAPI.Utils;
using WeatherServiceAPI.Models;
using WeatherServiceAPI.Busines;

namespace WeatherServiceAPI.Controllers
{
    public class ForecastController : ControllerBase
    {
        private readonly GetDataByCoordinates _getDataByCoordinates;

        //constructor
        public ForecastController(IConfiguration configuration) {
            _getDataByCoordinates = new GetDataByCoordinates(
                configuration["URLWeather"], 
                configuration["DBConnection"],
                configuration["DBname"],
                configuration["CollectionName"],
                configuration["URLCity"],
                configuration["ApiKey"]
            );
        }
        [HttpGet]
        [Route("ForecastByCoordinates")]
        public async Task<GeneralResponse<WeatherData>> consumirAsync([FromQuery] double latitude, double longitude)
        {
            Coordinates coordinates = new ();
            GeneralResponse<WeatherData> generalResponse = new ();
            try
            {
                //set coordinates
                coordinates.latitude = latitude;
                coordinates.longitude = longitude;
                generalResponse = await _getDataByCoordinates.GetForecastData(coordinates);
                return generalResponse;
            }
            catch (Exception ex) {
                generalResponse.isSuccess = false;
                generalResponse.message = ex.Message;
                return generalResponse;
            }
        }
        [HttpGet]
        [Route("ForecastByCityName")]
        public async Task<GeneralResponse<WatherDataByCityList<WeatherDataByCity>>> consumirCiudad([FromQuery] string city)
        {
            GeneralResponse<WatherDataByCityList<WeatherDataByCity>> generalResponse = new();
            try
            {
                generalResponse = await _getDataByCoordinates.ForecastByCity(city);
                return generalResponse;
            }
            catch (Exception ex)
            {
                generalResponse.isSuccess = false;
                generalResponse.message = ex.Message;
                return generalResponse;
            }
        }
    }
}
