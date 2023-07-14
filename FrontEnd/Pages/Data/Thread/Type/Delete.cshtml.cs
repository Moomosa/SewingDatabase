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

namespace FrontEnd.Pages.Data.Thread.Type
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
		public ThreadTypes ThreadTypes { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			ThreadTypes = await _apiService.GetSingleItem<ThreadTypes>(id.Value, userId);

			if (ThreadTypes == null)
				return NotFound();

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null || ThreadTypes == null)
				return NotFound();

			bool deleted = await _apiService.DeleteItem<ThreadTypes>(id.Value);
			if (!deleted)
			{
				TempData["DeleteFailureMessage"] = "Deletion is not allowed because it is used in a Thread.";
				return RedirectToPage("Delete", new { id });
			}

			return RedirectToPage("./Index");
		}
	}
}
