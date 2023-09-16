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
	public class DeleteModel : PageModel
	{
		private readonly ApiService _apiService;

		public DeleteModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		[BindProperty]
		public SewingModels.Models.Machine Machine { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			Machine = await _apiService.GetSingleItem<SewingModels.Models.Machine>(id.Value, userId);

			if (Machine == null)
				return NotFound();

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null || Machine == null)
				return NotFound();

			bool deleted = await _apiService.DeleteItem<SewingModels.Models.Machine>(id.Value);
			if (!deleted)
			{
				TempData["DeleteFailureMessage"] = "Deletion is not allowed.";
				return RedirectToPage("Delete", new { id });
			}

			HttpContext.Session.Remove("Machines");
			HttpContext.Session.Remove("MachineTotalRecords");

			return RedirectToPage("./Index");
		}
	}
}
