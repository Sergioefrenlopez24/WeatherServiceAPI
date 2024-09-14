using WeatherServiceAPI.Controllers.Models;
using WeatherServiceAPI.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization;
using System.Text.Json;
namespace WeatherServiceAPI.Utils
{
    public class Operations
    {
        private readonly string _bsaeUrl;
        private readonly string _parameters;
        public Operations(string bsaeUrl, string parameters)
        {
            _bsaeUrl = bsaeUrl;
            _parameters = parameters;
        }
        public async Task<String> ExtractDataFromMateo(float latitude, float longitude)
        {
            Coordinates data = new Coordinates();
            data.latitude = latitude;
            data.longitude = longitude;
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}latitude={1}&longitude={2}{3}", _bsaeUrl, data.latitude, data.longitude,_parameters));
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStringAsync();
                return resp;
            }
            catch (Exception ex) { 
                return ex.Message;
            }
        }
        public async Task<WeatherData>ExtractDataFromMongo(float latitude, float longitude)
        {
            WeatherData weatherData = new WeatherData();
            WeatherData JsonResponse = new WeatherData();
            Coordinates data = new Coordinates();
            data.latitude = latitude;
            data.longitude = longitude;
            var connectionString = "mongodb://localhost:27017"; 
            var client = new MongoClient(connectionString);

            // Seleccionar la base de datos
            var database = client.GetDatabase("WeatherService");

            // Seleccionar la colección
            var collection = database.GetCollection<BsonDocument>("Forecast");

            // Ejemplo de inserción de un documento

            // Ejemplo de consulta de documentos
            var filter = Builders<BsonDocument>.Filter.Eq("latitude", data.latitude);
            var result = collection.Find(filter).ToList();
            foreach (var item in result)
            {
                JsonResponse = BsonSerializer.Deserialize<WeatherData>(item);
            }          
                weatherData.generationtime_ms = JsonResponse.generationtime_ms;
                weatherData.latitude = JsonResponse.latitude;
                weatherData.longitude = JsonResponse.longitude;
                weatherData.utc_offset_seconds = JsonResponse.utc_offset_seconds;
                weatherData.timezone = JsonResponse.timezone;
                weatherData.hourly_units = JsonResponse.hourly_units;
                weatherData.hourly = JsonResponse.hourly;
                weatherData.daily_units = JsonResponse.daily_units;
                weatherData.daily = JsonResponse.daily;
            
            return weatherData;
        }
    }
}
