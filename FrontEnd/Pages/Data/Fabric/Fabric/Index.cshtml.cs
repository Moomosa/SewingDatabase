using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;
using System.Security.Claims;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace FrontEnd.Pages.Data.Fabric.Fabric
{
	[Authorize(Roles = "User,Admin")]
	public class IndexModel : PageModel
	{
		private readonly ApiService _apiService;

		public IndexModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public IList<SewingModels.Models.Fabric> Fabric { get; set; } = default!;
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

			if (!HttpContext.Session.TryGetValue("FabricTotalRecords", out byte[] totalRecordsBytes))
			{
				TotalRecords = await _apiService.GetTotalRecords<SewingModels.Models.Fabric>("Fabric", userId);
				HttpContext.Session.SetObjectAsJson("FabricTotalRecords", TotalRecords);
			}
			else
				TotalRecords = JsonConvert.DeserializeObject<int>(Encoding.UTF8.GetString(totalRecordsBytes));

			int lastPageVisited = HttpContext.Session.GetObjectFromJson<int>("FabricLastPage");

			string referrer = HttpContext.Request.Headers["Referer"];
			if (!string.IsNullOrEmpty(referrer) && referrer.Contains("Fabric/Fabric"))
			{
				if (CurrentPage != lastPageVisited) //This hits when changing pagination page
				{
					Fabric = await _apiService.GetPagedRecords<SewingModels.Models.Fabric>("Fabric", userId, CurrentPage, PageSize);
					HttpContext.Session.SetObjectAsJson("Fabrics", Fabric);
					HttpContext.Session.SetObjectAsJson("FabricLastPage", CurrentPage);
				}
				else //A refresh possibility
					Fabric = HttpContext.Session.GetObjectFromJson<IList<SewingModels.Models.Fabric>>("Fabrics");
			}
			else
			{
				if (lastPageVisited != 0) //This hits when coming from not this page
				{
					CurrentPage = lastPageVisited;
					Fabric = HttpContext.Session.GetObjectFromJson<IList<SewingModels.Models.Fabric>>("Fabrics");
				}
				else //This hits the first time coming to the page
				{
					Fabric = await _apiService.GetPagedRecords<SewingModels.Models.Fabric>("Fabric", userId, CurrentPage, PageSize);
					HttpContext.Session.SetObjectAsJson("Fabrics", Fabric);
				}
			}

			return Page();
		}
	}
}
