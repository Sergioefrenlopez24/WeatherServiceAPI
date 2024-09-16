using WeatherServiceAPI.Controllers.Models;
using WeatherServiceAPI.Models;
using WeatherServiceAPI.Utils;
namespace WeatherServiceAPI.Busines
{
    public class GetDataByCoordinates
    {
        private readonly string _bsaeUrl;
        private readonly string _dbConnection;
        private readonly string _DBname;
        private readonly string _collectionName;
        private readonly string _uRLCity;
        private readonly string _apiKey;
        private readonly ExtractDataFromMongoDB _extractDataFromMongoDB;
        private readonly ExtractDataFromExternalAPI _extractDataFromExternalAPI;
        public GetDataByCoordinates(string bsaeUrl,string dbConnection, string dBname, string collectionName, string uRLCity, string apiKey)
        {
            //Dependency injection
            _bsaeUrl = bsaeUrl;
            _dbConnection = dbConnection;
            _DBname = dBname;
            _collectionName = collectionName;
            _uRLCity = uRLCity;
            _apiKey = apiKey;
            _extractDataFromMongoDB = new ExtractDataFromMongoDB(_dbConnection, _DBname,_collectionName);
            _extractDataFromExternalAPI = new ExtractDataFromExternalAPI(_bsaeUrl, _apiKey,_uRLCity);
        }
        public async Task<GeneralResponse<WeatherData>> GetForecastData(Coordinates coordinates)
        {
            GeneralResponse<WeatherData> generalResponse = new();
            WeatherData weatherData = new();
            try
            {
                //Evaluates valid coordinates 
                if ((coordinates.latitude > -90 && coordinates.latitude < 90) && (coordinates.longitude > -180 && coordinates.longitude < 180))
                {
                    //extraxt data from database
                    weatherData = await _extractDataFromMongoDB.ExtractData(coordinates);                    
                    if (weatherData.daily == null) {
                        //if not found in, calls External API
                        weatherData = await _extractDataFromExternalAPI.ExtractData(coordinates);
                        generalResponse.isSuccess = true;
                        generalResponse.message = "Data obtained from External API";
                        generalResponse.data.Add(weatherData);
                        weatherData.latitude = coordinates.latitude;
                        weatherData.longitude = coordinates.longitude;
                        //insert obtained data into database 
                        GeneralResponse insertion = await _extractDataFromMongoDB.InsertData(weatherData);
                    }
                    else
                    {
                        generalResponse.isSuccess = true;
                        generalResponse.message = "Data obtained from database";
                        generalResponse.data.Add(weatherData);
                    }
                }
                else
                {
                    generalResponse.isSuccess = false;
                    generalResponse.message = " Invalid coordinates! The latitude should be between -90 and 90 degrees. The longitude should be between -180 and 180 degrees.";
                }
            }
            catch (Exception ex) { 
            generalResponse.isSuccess =false;
            generalResponse.message = ex.Message;
            }
            return generalResponse;
        }
        public async Task<GeneralResponse<WatherDataByCityList<WeatherDataByCity>>> ForecastByCity(string city)
        {
            CityList<City> cityList = new();
            Coordinates coordinates = new();
            WeatherData weatherData = new();
            GeneralResponse<WeatherData> generalResponse = new();
            WatherDataByCityList<WeatherDataByCity> watherDataByCityList = new();
            GeneralResponse<WatherDataByCityList<WeatherDataByCity>> response = new();

            try
            {
                //looks for the city at external API
                cityList.Cities = await _extractDataFromExternalAPI.ObtainCities(city);
                if(cityList.Cities.Count == 0) 
                {
                    response.isSuccess = false;
                    response.message = "City not found";
                    return response;
                }
                foreach (City item in cityList.Cities)
                {
                    //Fills the city's data 
                    WeatherDataByCity weatherDataByCity = new();
                    coordinates.latitude = item.latitude;
                    coordinates.longitude = item.longitude;
                    weatherDataByCity.name = item.name;
                    weatherDataByCity.state = item.state;
                    weatherDataByCity.country = item.country;
                    generalResponse = await GetForecastData(coordinates);
                    weatherDataByCity.Origin = generalResponse.message;
                    weatherDataByCity.WeatherData = generalResponse.data[0];
                    watherDataByCityList.WeatherByCities.Add(weatherDataByCity);
                }
                response.isSuccess = true;
                response.message = "cities found";
                response.data.Add(watherDataByCityList);
                return response;
            }
            catch (Exception e) { 
                response.isSuccess=false;
                response.message = e.Message;
                return response;
            }                
        }
    }
}
