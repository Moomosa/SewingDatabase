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

namespace FrontEnd.Pages.Data.Thread.Thread
{
	[Authorize(Roles = "User,Admin")]
	public class IndexModel : PageModel
	{
		private readonly ApiService _apiService;

		public IndexModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public IList<SewingModels.Models.Thread> Thread { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync()
		{
			Thread = HttpContext.Session.GetObjectFromJson<IList<SewingModels.Models.Thread>>("Threads");

			if (Thread == null)
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

				if (userId != null)
				{
					Thread = await _apiService.GetRecordsForUser<SewingModels.Models.Thread>("Thread", userId);
					HttpContext.Session.SetObjectAsJson("Threads", Thread);
				}
				else
					return RedirectToPage("/Account/Login");
			}
			return Page();
		}
	}
}
