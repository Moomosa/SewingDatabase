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

		public async Task<IActionResult> OnGetAsync()
		{
			Elastic = HttpContext.Session.GetObjectFromJson<IList<SewingModels.Models.Elastic>>("Elastics");

			if (Elastic == null)
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId != null)
				{
					Elastic = await _apiService.GetRecordsForUser<SewingModels.Models.Elastic>("Elastic", userId);
					HttpContext.Session.SetObjectAsJson("Elastics", Elastic);
				}
				else
					return RedirectToPage("/Account/Login");
			}
			return Page();
		}
	}
}
