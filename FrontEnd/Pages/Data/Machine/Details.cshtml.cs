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

namespace FrontEnd.Pages.Data.Machine
{
	[Authorize(Roles = "User,Admin")]
	public class DetailsModel : PageModel
	{
		private readonly ApiService _apiService;
		public DetailsModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public SewingModels.Models.Machine Machine { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null || _apiService == null)
				return NotFound();

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var machine = await _apiService.GetSingleItem<SewingModels.Models.Machine>(id.Value, userId);
			if (machine == null)
				return NotFound();
			else
				Machine = machine;

			return Page();
		}
	}
}
