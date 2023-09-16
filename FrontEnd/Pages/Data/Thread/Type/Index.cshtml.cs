using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Text;

namespace FrontEnd.Pages.Data.Thread.Type
{
	[Authorize(Roles = "User,Admin")]
	public class IndexModel : PageModel
	{
		private readonly ApiService _apiService;

		public IndexModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public IList<ThreadTypes> ThreadTypes { get; set; } = default!;
		[BindProperty(SupportsGet = true)]
		public int CurrentPage { get; set; } = 1;
		public int PageSize { get; set; } = 1;    //Set this to number of rows seen on page
		public int TotalRecords { get; set; } = -1;
		public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalRecords, PageSize));
		public bool ShowPrevious => CurrentPage > 1;
		public bool ShowNext => CurrentPage < TotalPages;
		public bool ShowFirst => CurrentPage != 1;
		public bool ShowLast => CurrentPage != TotalPages;

		public async Task<IActionResult> OnGetAsync()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null) return RedirectToPage("/Account/Login");

			if (!HttpContext.Session.TryGetValue("TTypesTotalRecords", out byte[] totalRecordsBytes))
			{
				TotalRecords = await _apiService.GetTotalRecords<ThreadTypes>("ThreadTypes", userId);
				HttpContext.Session.SetObjectAsJson("TTypesTotalRecords", TotalRecords);
			}
			else
				TotalRecords = JsonConvert.DeserializeObject<int>(Encoding.UTF8.GetString(totalRecordsBytes));

			int lastPageVisited = HttpContext.Session.GetObjectFromJson<int>("TTypesLastPage");

			string referrer = HttpContext.Request.Headers["Referer"];
			if (!string.IsNullOrEmpty(referrer) && referrer.Contains("Thread/Type"))
			{
				if (CurrentPage != lastPageVisited) //This hits when changing pagination page
				{
					ThreadTypes = await _apiService.GetPagedRecords<ThreadTypes>("ThreadTypes", userId, CurrentPage, PageSize);
					HttpContext.Session.SetObjectAsJson("ThreadTypes", ThreadTypes);
					HttpContext.Session.SetObjectAsJson("TTypesLastPage", CurrentPage);
				}
				else //A refresh possibility
					ThreadTypes = HttpContext.Session.GetObjectFromJson<IList<ThreadTypes>>("ThreadTypes");
			}
			else
			{
				if (lastPageVisited != 0) //This hits when coming from not this page
				{
					CurrentPage = lastPageVisited;
					ThreadTypes = HttpContext.Session.GetObjectFromJson<IList<ThreadTypes>>("ThreadTypes");
				}
				else //This hits the first time coming to the page
				{
					ThreadTypes = await _apiService.GetPagedRecords<ThreadTypes>("ThreadTypes", userId, CurrentPage, PageSize);
					HttpContext.Session.SetObjectAsJson("ThreadTypes", ThreadTypes);
				}
			}

			return Page();
		}
	}
}
