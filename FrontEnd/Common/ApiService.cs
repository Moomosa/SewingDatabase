using BackendDatabase.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SewingModels.Models;
using System.Text;
using System.Net.Http;
using ModelLibrary.Models.Database;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Configuration;

namespace FrontEnd
{
	public static class ApiService
	{
		private static HttpClient _httpClient;

		public static void Initialize(IConfiguration configuration)
		{
			string baseUrl = configuration.GetSection("AppSettings:BaseUrl").Value;

			_httpClient = new HttpClient()
			{
				BaseAddress = new Uri(baseUrl)
			};
		}

		public static async Task<List<T>> GetRecordsForUser<T>(string tableName, string userName) where T : class
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

		public static async Task<int> GetTotalRecords<T>(string tableName, string userId) where T : class
		{
			HttpResponseMessage countResponse = await _httpClient.GetAsync($"/api/{tableName}/count/{userId}");

			if (countResponse.IsSuccessStatusCode)
			{
				string countJson = await countResponse.Content.ReadAsStringAsync();
				int count = JsonConvert.DeserializeObject<int>(countJson);
				return count;
			}
			else
			{
				return 0;
			}
		}

		public static async Task<List<T>> GetPagedRecords<T>(string tabelName, string userId, int currentPage, int recordsPerPage) where T : class
		{
			HttpResponseMessage response = await _httpClient.GetAsync($"/api/{tabelName}/paged/{userId}/{currentPage}/{recordsPerPage}");

			if (response.IsSuccessStatusCode)
			{
				string resultJson = await response.Content.ReadAsStringAsync();
				List<T> values = JsonConvert.DeserializeObject<List<T>>(resultJson);
				return values;
			}
			else
			{
				return new List<T>();
			}
		}

		public static async Task<List<T>> GatherAllRecords<T>(string tableName, string userId, int chunkSize) where T : class
		{
			List<T> allRecords = new List<T>();
			int currentPage = 1;

			while (true)
			{
				HttpResponseMessage response = await _httpClient.GetAsync($"api/{tableName}/paged/{userId}/{currentPage}/{chunkSize}");

				if (response.IsSuccessStatusCode)
				{
					string result = await response.Content.ReadAsStringAsync();
					List<T> chunk = JsonConvert.DeserializeObject<List<T>>(result);

					if (chunk.Count == 0)
						break;

					allRecords.AddRange(chunk);

					if (chunk.Count < chunkSize)
						break;

					currentPage++;
				}
				else
					break;
			}

			return allRecords;
		}

		public static async Task<HttpResponseMessage> PostNewItem<T>(T item, string url, string userId) where T : class
		{
			var json = JsonConvert.SerializeObject(item);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			string fullUrl = $"{url}?userId={userId}";

			return await _httpClient.PostAsync(fullUrl, content);
		}

		public static async Task<bool> DeleteItem<T>(int id) where T : class
		{
			string tableName = typeof(T).Name;

			HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/{tableName}/{id}");

			return response.IsSuccessStatusCode;
		}

		public static async Task<T> GetSingleItem<T>(int id, string userId) where T : class
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

		public static async Task<bool> UpdateItem<T>(int id, T item) where T : class
		{
			var json = JsonConvert.SerializeObject(item);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync($"/api/{typeof(T).Name}/{id}", content);
			return response.IsSuccessStatusCode;
		}

		public static async Task<MultiDataDTO> GetMultiData(string userName, int count)
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
