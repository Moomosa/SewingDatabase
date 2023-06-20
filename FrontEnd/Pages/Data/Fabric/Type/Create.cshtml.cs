using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BackendDatabase.Data;
using SewingModels.Models;
using System.Security.Claims;

namespace FrontEnd.Pages.Data.Fabric.Type
{
	public class CreateModel : PageModel
	{
		private readonly BackendDatabase.Data.BackendDatabaseContext _context;
		private readonly ApiService _apiService;

		public CreateModel(BackendDatabase.Data.BackendDatabaseContext context, ApiService apiService)
		{
			_context = context;
			_apiService = apiService;
		}

		public IActionResult OnGet()
		{
			return Page();
		}

		[BindProperty]
		public FabricTypes FabricTypes { get; set; } = default!;


		// To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid || _context.FabricTypes == null || FabricTypes == null)
			{
				return Page();
			}
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			HttpResponseMessage response = await _apiService.PostNewItem(FabricTypes, "/api/FabricTypes", userId);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToPage("./Index");
			}
			else
			{
				ModelState.AddModelError("", "Failed to create item");
				return Page();
			}
		}
	}
}
