namespace WeatherServiceAPI.Models
{
    public class GeneralResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public GeneralResponse()
        {
            isSuccess = false;
            message = string.Empty;
        }
        public GeneralResponse(bool status, string msg)
        {
            isSuccess = status;
            message = msg;
        }
        
    }
    public class GeneralResponse<T>
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public List<T> data { get; set; }
        public GeneralResponse()
        {
            data = new List<T>();
        }
    }
}
