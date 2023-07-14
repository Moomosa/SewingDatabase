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

namespace FrontEnd.Pages.Data.Fabric.Fabric
{
	[Authorize(Roles = "User,Admin")]
	public class DetailsModel : PageModel
	{
		private readonly ApiService _apiService;

		public DetailsModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public SewingModels.Models.Fabric Fabric { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null || _apiService == null)
				return NotFound();

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var fabric = await _apiService.GetSingleItem<SewingModels.Models.Fabric>(id.Value, userId);
			if (fabric == null)
				return NotFound();
			else
				Fabric = fabric;

			return Page();
		}
	}
}
