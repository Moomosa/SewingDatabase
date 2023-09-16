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

namespace FrontEnd.Pages.Data.Elastic.Elastic
{
	[Authorize(Roles = "User,Admin")]
	public class IndexModel : PageModel
	{
		private readonly ApiService _apiService;

		public IndexModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public IList<SewingModels.Models.Elastic> Elastic { get; set; } = default!;
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


			if (!HttpContext.Session.TryGetValue("ElasticTotalRecords", out byte[] totalRecordsBytes))
			{
				TotalRecords = await _apiService.GetTotalRecords<SewingModels.Models.Elastic>("Elastic", userId);
				HttpContext.Session.SetObjectAsJson("ElasticTotalRecords", TotalRecords);
			}
			else
				TotalRecords = JsonConvert.DeserializeObject<int>(Encoding.UTF8.GetString(totalRecordsBytes));

			int lastPageVisited = HttpContext.Session.GetObjectFromJson<int>("ElasticLastPage");

			string referrer = HttpContext.Request.Headers["Referer"];
			if (!string.IsNullOrEmpty(referrer) && referrer.Contains("Elastic/Elastic"))
			{
				if (CurrentPage != lastPageVisited) //This hits when changing pagination page
				{
					Elastic = await _apiService.GetPagedRecords<SewingModels.Models.Elastic>("Elastic", userId, CurrentPage, PageSize);
					HttpContext.Session.SetObjectAsJson("Elastics", Elastic);
					HttpContext.Session.SetObjectAsJson("ElasticLastPage", CurrentPage);
				}
				else //A refresh possibility
					Elastic = HttpContext.Session.GetObjectFromJson<IList<SewingModels.Models.Elastic>>("Elastics");
			}
			else
			{
				if (lastPageVisited != 0) //This hits when coming from not this page
				{
					CurrentPage = lastPageVisited;
					Elastic = HttpContext.Session.GetObjectFromJson<IList<SewingModels.Models.Elastic>>("Elastics");
				}
				else //This hits the first time coming to the page
				{
					Elastic = await _apiService.GetPagedRecords<SewingModels.Models.Elastic>("Elastic", userId, CurrentPage, PageSize);
					HttpContext.Session.SetObjectAsJson("Elastics", Elastic);
				}
			}
			return Page();
		}
	}
}
