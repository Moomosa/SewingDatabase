using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Drawing.Printing;
using System.Text;

namespace FrontEnd
{
    public class FrontHelpers
    {
        public async Task<int> GetTotalRecords<T>(string type, string userId, IHttpContextAccessor httpContextAccessor, ApiService apiService) where T : class
        {
            int totalRecords;

            if (!httpContextAccessor.HttpContext.Session.TryGetValue($"{type}TotalRecords", out byte[] totalRecordsBytes))
            {
                totalRecords = await apiService.GetTotalRecords<T>(type, userId);
                httpContextAccessor.HttpContext.Session.SetObjectAsJson($"{type}TotalRecords", totalRecords);
            }
            else
                totalRecords = JsonConvert.DeserializeObject<int>(Encoding.UTF8.GetString(totalRecordsBytes));

            return totalRecords;
        }

        public int GetLastPageVisited(string typeName, IHttpContextAccessor httpContextAccessor)
        {            
            return httpContextAccessor.HttpContext.Session.GetObjectFromJson<int>($"{typeName}LastPageVisited");
        }

        public int GetCurrentPageSize(string typeName, IHttpContextAccessor httpContextAccessor)
        {
            if(httpContextAccessor.HttpContext.Session.TryGetValue($"{typeName}PageSize", out byte[] pageSizeBytes))            
				if (int.TryParse(Encoding.UTF8.GetString(pageSizeBytes), out int pageSize))				
					return pageSize;

            return 5;
        }

        public int GetCurrentPage(string typeName, IHttpContextAccessor httpContextAccessor)
        {
			if (httpContextAccessor.HttpContext.Session.TryGetValue($"{typeName}LastPageVisited", out byte[] currentPageBytes))
				if (int.TryParse(Encoding.UTF8.GetString(currentPageBytes), out int currentPage))
					return currentPage;

			return 1;

		}

		public void SetPageValues(string type, int currentPage, int pageSize, IHttpContextAccessor httpContextAccessor)
        {
			var session = httpContextAccessor.HttpContext.Session;
            session.SetObjectAsJson($"{type}PageSize", pageSize);
			session.SetObjectAsJson($"{type}LastPageVisited", currentPage);
		}

		public async Task<IList<T>> CallForRecords<T>(string type, string userId, int currentPage, int pageSize, IHttpContextAccessor httpContextAccessor, ApiService apiService) where T : class
        {
            var items = await apiService.GetPagedRecords<T>(type, userId, currentPage, pageSize);
            var session = httpContextAccessor.HttpContext.Session;
            session.SetObjectAsJson($"{type}Records", items);
            session.SetObjectAsJson($"{type}LastPageVisited", currentPage);
            return items;
        }

        public IList<T> GetRecordsFromSession<T>(string type, IHttpContextAccessor httpContextAccessor) where T : class
        {
            return httpContextAccessor.HttpContext.Session.GetObjectFromJson<IList<T>>($"{type}Records");
        }

        public void ClearSessionData(string type, IHttpContextAccessor httpContextAccessor)
        {
            var session = httpContextAccessor.HttpContext.Session;
            session.Remove($"{type}Records");
            session.Remove($"{type}TotalRecords");
			session.SetObjectAsJson($"{type}LastPageVisited", 0);			
        }
    }
}
