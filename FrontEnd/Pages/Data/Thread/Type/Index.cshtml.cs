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

		public async Task<IActionResult> OnGetAsync()
		{
			ThreadTypes = HttpContext.Session.GetObjectFromJson<IList<ThreadTypes>>("TTypes");

			if (ThreadTypes == null)
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId != null)
				{
					ThreadTypes = await _apiService.GetRecordsForUser<ThreadTypes>("ThreadTypes", userId);
					HttpContext.Session.SetObjectAsJson("TTypes", ThreadTypes);
				}
				else
					return RedirectToPage("/Account/Login");
			}
			return Page();
		}
	}
}
