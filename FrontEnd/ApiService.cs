namespace FrontEnd
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        public ApiService(IConfiguration configuration)
        {
            string baseUrl = configuration.GetSection("AppSettings:BaseUrl").Value;

            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl)
            };
        }
    }
}
