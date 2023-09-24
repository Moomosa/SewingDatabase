using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;
using System.Security.Claims;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace FrontEnd.Pages.Data.Fabric.Item
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
		public SewingModels.Models.Fabric Fabric { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			Fabric = await _apiService.GetSingleItem<SewingModels.Models.Fabric>(id.Value, userId);

			if (Fabric == null)
				return NotFound();

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null || Fabric == null)
				return NotFound();

			bool deleted = await _apiService.DeleteItem<SewingModels.Models.Fabric>(id.Value);
			if (!deleted)
				return NotFound();

			HttpContext.Session.Remove("Fabrics");
			HttpContext.Session.Remove("FabricTotalRecords");

			return RedirectToPage("./Index");
		}
	}
}
