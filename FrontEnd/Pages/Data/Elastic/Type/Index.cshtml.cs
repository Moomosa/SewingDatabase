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

namespace FrontEnd.Pages.Data.Elastic.Type
{
	[Authorize(Roles = "User,Admin")]
	public class IndexModel : PageModel
	{
		private readonly ApiService _apiService;

		public IndexModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public IList<ElasticTypes> ElasticTypes { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync()
		{
			ElasticTypes = HttpContext.Session.GetObjectFromJson<IList<ElasticTypes>>("ETypes");

			if (ElasticTypes == null)
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId != null)
				{
					ElasticTypes = await _apiService.GetRecordsForUser<ElasticTypes>("ElasticTypes", userId);
					HttpContext.Session.SetObjectAsJson("ETypes", ElasticTypes);
				}
				else
					return RedirectToPage("/Account/Login");
			}
			return Page();
		}
	}
}
