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

namespace FrontEnd.Pages.Data.Thread.Thread
{
	[Authorize(Roles = "User,Admin")]
	public class DeleteModel : PageModel
	{
		private readonly ApiService _apiService;

		public DeleteModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		[BindProperty]
		public SewingModels.Models.Thread Thread { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			Thread = await _apiService.GetSingleItem<SewingModels.Models.Thread>(id.Value, userId);

			if (Thread == null)
				return NotFound();

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null || Thread == null)
				return NotFound();

			bool deleted = await _apiService.DeleteItem<SewingModels.Models.Thread>(id.Value);
			if (!deleted)
				return NotFound();

			HttpContext.Session.Remove("Threads");
			HttpContext.Session.Remove("ThreadTotalRecords");

			return RedirectToPage("./Index");
		}
	}
}
