using BackendDatabase.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SewingModels.Models;
using System.Text;
using System.Net.Http;
using ModelLibrary.Models.Database;

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

		public async Task<List<T>> GetRecordsForUser<T>(string tableName, string userName) where T : class
		{
			HttpResponseMessage recordResponse = await _httpClient.GetAsync($"/api/{tableName}/byIds/{tableName}/{userName}");

			if (recordResponse.IsSuccessStatusCode)
			{
				string json = await recordResponse.Content.ReadAsStringAsync();
				List<T> recordIds = JsonConvert.DeserializeObject<List<T>>(json);

				if (recordIds.Count == 0)
					return new List<T>();

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

		public async Task<T> GetSingleItem<T>(int id, string userId) where T : class
		{
			HttpResponseMessage response = await _httpClient.GetAsync($"/api/{typeof(T).Name}/{id}/{userId}");

			if (response.IsSuccessStatusCode)
			{
				string content = await response.Content.ReadAsStringAsync();
				T item = JsonConvert.DeserializeObject<T>(content);
				return item;
			}
			else
				return null;
		}

		public async Task<bool> UpdateItem<T>(int id, T item) where T : class
		{
			var json = JsonConvert.SerializeObject(item);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync($"/api/{typeof(T).Name}/{id}", content);
			return response.IsSuccessStatusCode;
		}

		public async Task<MultiDataDTO> GetMultiData(string userName, int count)
		{
			HttpResponseMessage response = await _httpClient.GetAsync($"api/Multi/{userName}/{count}");

			if (response.IsSuccessStatusCode)
			{
				string json = await response.Content.ReadAsStringAsync();
				MultiDataDTO multiData = JsonConvert.DeserializeObject<MultiDataDTO>(json);
				return multiData;
			}
			else
			{
				return null;
			}
		}
	}

	public static class SessionExtensions
	{
		public static void SetObjectAsJson(this ISession session, string key, object value)
		{
			session.SetString(key, JsonConvert.SerializeObject(value));
		}
		public static T GetObjectFromJson<T>(this ISession session, string key)
		{
			var value = session.GetString(key);
			return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
		}
	}

}
