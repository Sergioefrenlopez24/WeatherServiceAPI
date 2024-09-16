using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using WeatherServiceAPI.Controllers.Models;
using WeatherServiceAPI.Models;

namespace WeatherServiceAPI.Utils
{
    public class ExtractDataFromMongoDB
    {
        private readonly string _dbConnection;
        private readonly string _DBname;
        private readonly string _collectionName;
        private readonly IMongoDatabase _database;
        public ExtractDataFromMongoDB(string dbConnection, string dBname, string collectionName)
        {
            _dbConnection = dbConnection;
            _DBname = dBname;
            _collectionName = collectionName;
            var client = new MongoClient(_dbConnection);
            _database = client.GetDatabase(_DBname);           
        }
        public async Task<WeatherData> ExtractData(Coordinates coordinates)
        {
            WeatherData weatherData = new();
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("yyyy-MM-ddTHH:mm");
            string dateWitoutMinutes = formattedDate.Substring(0, formattedDate.Length - 2) + "00";
            var collection = _database.GetCollection<BsonDocument>(_collectionName);
            //create a filter with latitude, longitude and time
            var filter = Builders<BsonDocument>.Filter.Eq("latitude", coordinates.latitude) &
                         Builders<BsonDocument>.Filter.Eq("longitude", coordinates.longitude)&
                         Builders<BsonDocument>.Filter.In("hourly.time", new []{ dateWitoutMinutes });
            var result = collection.Find(filter).ToList();
            if (result.Count > 0)
            {
                weatherData = BsonSerializer.Deserialize<WeatherData>(result[0]);
                return weatherData;
            }
            else
            {
                return weatherData;
            }           
        }
        public async Task<GeneralResponse>InsertData(WeatherData weatherData)
        {
            //insert data into database 
            GeneralResponse generalResponse = new();
            try
            {
                var collection = _database.GetCollection<BsonDocument>(_collectionName);
                collection.InsertOne(weatherData.ToBsonDocument());
                generalResponse.isSuccess = true;
                generalResponse.message = "Succesfull insertion";
                return generalResponse;
            }
            catch (Exception ex) { 
                generalResponse.isSuccess = false;
                generalResponse.message = ex.Message;
                return generalResponse;
            }

        }
    }
}
