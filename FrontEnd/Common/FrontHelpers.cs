using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Drawing.Printing;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;

namespace FrontEnd
{
	public static class FrontHelpers
	{
		public static async Task<int> GetTotalRecords<T>(string type, string userId, IHttpContextAccessor httpContextAccessor) where T : class
		{
			int totalRecords;

			if (!httpContextAccessor.HttpContext.Session.TryGetValue($"{type}TotalRecords", out byte[] totalRecordsBytes))
			{
				totalRecords = await ApiService.GetTotalRecords<T>(type, userId);
				httpContextAccessor.HttpContext.Session.SetObjectAsJson($"{type}TotalRecords", totalRecords);
			}
			else
				totalRecords = JsonConvert.DeserializeObject<int>(Encoding.UTF8.GetString(totalRecordsBytes));

			return totalRecords;
		}

		public static int GetLastPageVisited(string typeName, IHttpContextAccessor httpContextAccessor)
		{
			return httpContextAccessor.HttpContext.Session.GetObjectFromJson<int>($"{typeName}LastPageVisited");
		}

		public static int GetCurrentPageSize(string typeName, IHttpContextAccessor httpContextAccessor)
		{
			if (httpContextAccessor.HttpContext.Session.TryGetValue($"{typeName}PageSize", out byte[] pageSizeBytes))
				if (int.TryParse(Encoding.UTF8.GetString(pageSizeBytes), out int pageSize))
					return pageSize;

			return 5;
		}

		public static int GetCurrentPage(string typeName, IHttpContextAccessor httpContextAccessor)
		{
			if (httpContextAccessor.HttpContext.Session.TryGetValue($"{typeName}LastPageVisited", out byte[] currentPageBytes))
				if (int.TryParse(Encoding.UTF8.GetString(currentPageBytes), out int currentPage))
					return currentPage;

			return 1;

		}

		public static void SetPageValues(string type, int currentPage, int pageSize, IHttpContextAccessor httpContextAccessor)
		{
			var session = httpContextAccessor.HttpContext.Session;
			session.SetObjectAsJson($"{type}PageSize", pageSize);
			session.SetObjectAsJson($"{type}LastPageVisited", currentPage);
		}

		public static async Task<IList<T>> CallForRecords<T>(string type, string userId, int currentPage, int pageSize, IHttpContextAccessor httpContextAccessor) where T : class
		{
			var items = await ApiService.GetPagedRecords<T>(type, userId, currentPage, pageSize);
			var session = httpContextAccessor.HttpContext.Session;
			session.SetObjectAsJson($"{type}Records", items);
			session.SetObjectAsJson($"{type}LastPageVisited", currentPage);
			return items;
		}		

		public static IList<T> GetRecordsFromSession<T>(string type, IHttpContextAccessor httpContextAccessor) where T : class
		{
			return httpContextAccessor.HttpContext.Session.GetObjectFromJson<IList<T>>($"{type}Records");
		}

		public static void ClearSessionData(string type, IHttpContextAccessor httpContextAccessor)
		{
			var session = httpContextAccessor.HttpContext.Session;
			session.Remove($"{type}Records");
			session.Remove($"{type}TotalRecords");
			session.SetObjectAsJson($"{type}LastPageVisited", 0);
		}

		public static string GetUserId(ClaimsPrincipal user)
		{
			return user.FindFirstValue(ClaimTypes.NameIdentifier);
		}
	}
}
