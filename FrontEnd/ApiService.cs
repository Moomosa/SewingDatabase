using Newtonsoft.Json;
using SewingModels.Models;

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

        public async Task<List<Fabric>> GetAllFabrics()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/Fabrics");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<Fabric> fabrics = JsonConvert.DeserializeObject<List<Fabric>>(json);
                return fabrics;
            }
            else
            {
                // Handle error response
                return null;
            }
        }


    }
}
