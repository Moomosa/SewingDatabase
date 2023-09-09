using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Misc.MiscObjects
{
	[Authorize(Roles = "User,Admin")]
	public class IndexModel : PageModel
	{
		private readonly ApiService _apiService;

		public IndexModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public IList<SewingModels.Models.MiscObjects> MiscObjects { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync()
		{
			MiscObjects = HttpContext.Session.GetObjectFromJson<IList<SewingModels.Models.MiscObjects>>("MiscItems");

			if (MiscObjects == null)
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId != null)
				{
					MiscObjects = await _apiService.GetRecordsForUser<SewingModels.Models.MiscObjects>("MiscObjects", userId);
					HttpContext.Session.SetObjectAsJson("MiscItems", MiscObjects);
				}
				else
					return RedirectToPage("/Account/Login");
			}
			return Page();
		}
	}
}
