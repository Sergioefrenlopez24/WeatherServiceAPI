namespace WeatherServiceAPI.Helper
{
    public class HttpHelper
    {

        public async Task<string> HttpRequest(string url, string key)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(url));
            if (key != null)
            {
                request.Headers.Add("x-api-key", string.Format(key));
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
