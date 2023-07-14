using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FrontEnd.Pages.Data.Machine
{
	[Authorize(Roles = "User,Admin")]
	public class EditModel : PageModel
	{
		private readonly ApiService _apiService;

		public EditModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		[BindProperty]
		public SewingModels.Models.Machine Machine { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null || _apiService == null)
				return NotFound();

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var machine = await _apiService.GetSingleItem<SewingModels.Models.Machine>(id.Value, userId);
			if (machine == null)
				return NotFound();

			Machine = machine;
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();

			try
			{
				bool updated = await _apiService.UpdateItem<SewingModels.Models.Machine>(Machine.ID, Machine);
				if (!updated)
					return NotFound();

				return RedirectToPage("./Index");
			}
			catch
			{
				return StatusCode(500, "An error occured while updating the item.");
			}
		}
	}
}
