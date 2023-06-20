using BackendDatabase.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SewingModels.Models;
using System.Text;
using System.Net.Http;

namespace FrontEnd
{
	public class ApiService
	{
		private readonly HttpClient _httpClient;
		private readonly BackendDatabaseContext _context;

		public ApiService(IConfiguration configuration, BackendDatabaseContext context)
		{
			string baseUrl = configuration.GetSection("AppSettings:BaseUrl").Value;
			_context = context;

			_httpClient = new HttpClient()
			{
				BaseAddress = new Uri(baseUrl)
			};
		}

		public async Task<List<T>> GetRecordsForUser<T>(string tableName, string userName) where T : class
		{
			HttpResponseMessage recordResponse = await _httpClient.GetAsync($"/api/{tableName}/{tableName}/{userName}");

			if (recordResponse.IsSuccessStatusCode)
			{
				string json = await recordResponse.Content.ReadAsStringAsync();
				List<T> recordIds = JsonConvert.DeserializeObject<List<T>>(json);

				if (recordIds.Count == 0)
				{
					return new List<T>();
				}

				return recordIds;
			}
			else
			{
				//Handle error
				return null;
			}
		}

		public async Task<HttpResponseMessage> PostNewItem<T>(T item, string url, string userId) where T : class
		{
			var json = JsonConvert.SerializeObject(item);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			string fullUrl = $"{url}?userId={userId}";

			return await _httpClient.PostAsync(fullUrl, content);
		}

		public async Task<bool> DeleteItem<T>(int id) where T : class
		{
			string tableName = typeof(T).Name;

			HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/{tableName}/{id}");
			return response.IsSuccessStatusCode;
		}
	}
}
