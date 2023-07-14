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

namespace FrontEnd.Pages.Data.Fabric.Type
{
	[Authorize(Roles = "User,Admin")]
	public class DetailsModel : PageModel
	{
		private readonly ApiService _apiService;
		public DetailsModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public FabricTypes FabricTypes { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null || _apiService == null)
				return NotFound();

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var fabrictypes = await _apiService.GetSingleItem<FabricTypes>(id.Value, userId);
			if (fabrictypes == null)    //TODO: Fix to not be NotFound, something like "Oops, this doesn't exist"
				return NotFound();
			else
				FabricTypes = fabrictypes;

			return Page();
		}
	}
}
