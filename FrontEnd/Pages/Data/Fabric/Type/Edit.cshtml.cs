using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Fabric.Type
{
	public class EditModel : PageModel
	{
		private readonly ApiService _apiService;

		public EditModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		[BindProperty]
		public FabricTypes FabricTypes { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null || _apiService == null)
			{
				return NotFound();
			}

			var fabrictypes = await _apiService.GetSingleItem<FabricTypes>(id.Value);
			if (fabrictypes == null)
			{
				return NotFound();
			}
			FabricTypes = fabrictypes;
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();

			try
			{
				bool updated = await _apiService.UpdateItem<FabricTypes>(FabricTypes.ID, FabricTypes);
				if (!updated)
					return NotFound();

				return RedirectToPage("./Index");
			}
			catch (Exception ex)
			{
				return StatusCode(500, "An error occured while updating the item.");
			}
		}

	}
}
