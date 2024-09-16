using WeatherServiceAPI.Controllers.Models;
using WeatherServiceAPI.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization;
using System.Text.Json;
using System;
using Microsoft.AspNetCore.Mvc;
namespace WeatherServiceAPI.Utils
{
    public class Operations
    {
        private readonly string _bsaeUrl;
        private readonly string _parameters;
        private readonly string _dbConnection;
        private readonly string _DBname;
        private readonly string _collectionName;
        private readonly string _URLCity;
        private readonly string _ApiKey;
        public Operations(string bsaeUrl, string parameters, string dbConnection, string dBname, string collectionName, string uRLCity, string apiKey)
        {
            _bsaeUrl = bsaeUrl;
            _parameters = parameters;
            _dbConnection = dbConnection;
            _DBname = dBname;
            _collectionName = collectionName;
            _URLCity = uRLCity;
            _ApiKey = apiKey;
        }
        public async Task<GeneralResponse> ExtractData(Coordinates coordinates)
        {
            WeatherData weatherData = new();
            GeneralResponse generalResponse = new();
            if ((coordinates.latitude > -90 && coordinates.latitude < 90) && (coordinates.longitude > -180 && coordinates.longitude < 180))
            {
                weatherData = await ExtractDataFromMongoDB(coordinates);
            }
            else
            {
                generalResponse.isSuccess = false;
                generalResponse.message = " Invalid coordinates! The latitude should be between -90 and 90 degrees. The longitude should be between -180 and 180 degrees.";

            }
            return generalResponse;
        }
        public async Task<WeatherDataList<WeatherData>> ConsumirCiudad(string ciudad)
        {
            CityList<City> city = new CityList<City>();
            Coordinates coordinates = new Coordinates();
            WeatherData weatherData = new WeatherData();
            WeatherDataList<WeatherData> weatherdatas = new();
            string resultCity = await HttpRequest(string.Format(_URLCity, ciudad), _ApiKey);
            city.Cities = JsonConvert.DeserializeObject<List<City>>(resultCity);

            foreach (City item in city.Cities)
            {
                coordinates.latitude = item.latitude;
                coordinates.longitude = item.longitude;
                weatherData = await ExtractDataFromMongoDB(coordinates);
                weatherdatas.weatherDataS.Add(weatherData);
            }
            return weatherdatas;

        }
        public async Task<WeatherData> ExtractDataFromMateo(Coordinates coordinates)
        {
                WeatherData weatherData = new();
                var resp = await HttpRequest(string.Format(_bsaeUrl, coordinates.latitude, coordinates.longitude),"");
                weatherData = JsonConvert.DeserializeObject<WeatherData>(resp);
                return weatherData;            
        }
        public async Task<WeatherData> ExtractDataFromMongoDB(Coordinates coordinates)
        {
            WeatherData weatherData = new ();
            var client = new MongoClient(_dbConnection);
            var database = client.GetDatabase(_DBname);
            var collection = database.GetCollection<BsonDocument>(_collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("latitude", coordinates.latitude) & 
                         Builders<BsonDocument>.Filter.Eq("longitude", coordinates.longitude);
            var result = collection.Find(filter).ToList();
            if (result.Count > 0)
            {
                weatherData = BsonSerializer.Deserialize<WeatherData>(result[0]);
            }
            else
            {
                weatherData = await ExtractDataFromMateo(coordinates);
                collection.InsertOne(weatherData.ToBsonDocument());
            }
            return weatherData;
        }

        public async Task<string> HttpRequest(string url, string key)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(url));
            if (key != null)
            {
                request.Headers.Add("x-api-key", _ApiKey);
            }            
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                throw new ArgumentException(result);
            }
        }
    }
}
