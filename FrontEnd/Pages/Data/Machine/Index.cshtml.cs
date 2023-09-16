using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BackendDatabase.Controllers.Machine;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Machine
{
	[Authorize(Roles = "User,Admin")]
	public class IndexModel : PageModel
	{
		private readonly ApiService _apiService;

		public IndexModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public IList<SewingModels.Models.Machine> Machine { get; set; } = default!;
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

			if (!HttpContext.Session.TryGetValue("MachineTotalRecords", out byte[] totalRecordsBytes))
			{
				TotalRecords = await _apiService.GetTotalRecords<SewingModels.Models.Machine>("Machine", userId);
				HttpContext.Session.SetObjectAsJson("MachineTotalRecords", TotalRecords);
			}
			else
				TotalRecords = JsonConvert.DeserializeObject<int>(Encoding.UTF8.GetString(totalRecordsBytes));

			int lastPageVisited = HttpContext.Session.GetObjectFromJson<int>("MachineLastPage");

			string referrer = HttpContext.Request.Headers["Referer"];
			if (!string.IsNullOrEmpty(referrer) && referrer.Contains("Data/Machine"))
			{
				if (CurrentPage != lastPageVisited) //This hits when changing pagination page
				{
					Machine = await _apiService.GetPagedRecords<SewingModels.Models.Machine>("Machine", userId, CurrentPage, PageSize);
					HttpContext.Session.SetObjectAsJson("Machines", Machine);
					HttpContext.Session.SetObjectAsJson("MachineLastPage", CurrentPage);
				}
				else //A refresh possibility
					Machine = HttpContext.Session.GetObjectFromJson<IList<SewingModels.Models.Machine>>("Machines");
			}
			else
			{
				if (lastPageVisited != 0) //This hits when coming from not this page
				{
					CurrentPage = lastPageVisited;
					Machine = HttpContext.Session.GetObjectFromJson<IList<SewingModels.Models.Machine>>("Machines");
				}
				else //This hits the first time coming to the page
				{
					Machine = await _apiService.GetPagedRecords<SewingModels.Models.Machine>("Machine", userId, CurrentPage, PageSize);
					HttpContext.Session.SetObjectAsJson("Machines", Machine);
				}
			}

			return Page();
		}
	}
}
