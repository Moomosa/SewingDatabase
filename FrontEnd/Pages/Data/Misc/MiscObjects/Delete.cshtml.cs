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

namespace FrontEnd.Pages.Data.Misc.MiscObjects
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
		public SewingModels.Models.MiscObjects MiscObjects { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			MiscObjects = await _apiService.GetSingleItem<SewingModels.Models.MiscObjects>(id.Value, userId);

			if (MiscObjects == null)
				return NotFound();

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null || MiscObjects == null)
				return NotFound();

			bool deleted = await _apiService.DeleteItem<SewingModels.Models.MiscObjects>(id.Value);
			if (!deleted)
				return NotFound();

			HttpContext.Session.Remove("MObjects");
			HttpContext.Session.Remove("MOTotalRecords");

			return RedirectToPage("./Index");
		}
	}
}
