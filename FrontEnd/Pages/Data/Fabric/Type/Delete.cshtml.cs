using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;
using System.Security.Claims;

namespace FrontEnd.Pages.Data.Fabric.Type
{
	public class DeleteModel : PageModel
	{
		private readonly ApiService _apiService;

		public DeleteModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		[BindProperty]
		public FabricTypes FabricTypes { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			FabricTypes = await _apiService.GetSingleItem<FabricTypes>(id.Value);

			if (FabricTypes == null)
				return NotFound();

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null || FabricTypes == null)
				return NotFound();

			bool deleted = await _apiService.DeleteItem<FabricTypes>(id.Value);
			if (!deleted)
				return NotFound();

			return RedirectToPage("./Index");
		}
	}
}
