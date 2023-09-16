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

namespace FrontEnd.Pages.Data.Fabric.Brand
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
		public FabricBrand FabricBrand { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)			
				return NotFound();

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			FabricBrand = await _apiService.GetSingleItem<FabricBrand>(id.Value, userId);

			if (FabricBrand == null)			
				return NotFound();
			
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null || FabricBrand == null)			
				return NotFound();

			bool deleted = await _apiService.DeleteItem<FabricBrand>(id.Value);
			if (!deleted)
			{
				//This message is sent to the page as the error message
				TempData["DeleteFailureMessage"] = "Deletion is not allowed because it is used in a Fabric.";
				return RedirectToPage("Delete", new { id });
			}

			HttpContext.Session.Remove("FBrands");
			HttpContext.Session.Remove("FBrandTotalRecords");

			return RedirectToPage("./Index");
		}
	}
}
