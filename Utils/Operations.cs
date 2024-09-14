using WeatherServiceAPI.Controllers.Models;
using WeatherServiceAPI.Models;
namespace WeatherServiceAPI.Utils
{
    public class Operations
    {
        private readonly string _bsaeUrl;
        public Operations(string bsaeUrl)
        {
            _bsaeUrl = bsaeUrl;
        }
        public async Task<String> ExtractData(float latitude, float longitude)
        {
            Coordinates data = new Coordinates();
            data.latitude = latitude;
            data.longitude = longitude;
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}latitude={1}&longitude={1}&hourly=temperature_2m&hourly=wind_direction_10m&hourly=wind_speed_10m&daily=sunrise", _bsaeUrl, data.latitude, data.latitude));
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStringAsync();
                return resp;
            }
            catch (Exception ex) { 
                return ex.Message;
            }
        }
    }
}
