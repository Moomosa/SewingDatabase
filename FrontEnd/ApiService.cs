namespace FrontEnd
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        public ApiService(string baseUrl)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl)
            };
        }
    }
}
