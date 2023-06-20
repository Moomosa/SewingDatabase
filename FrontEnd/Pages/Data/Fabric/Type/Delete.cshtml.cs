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

namespace FrontEnd.Pages.Data.Fabric.Type
{
	public class DeleteModel : PageModel
	{
		private readonly BackendDatabase.Data.BackendDatabaseContext _context;
		private readonly ApiService _apiService;

		public DeleteModel(BackendDatabase.Data.BackendDatabaseContext context, ApiService apiService)
		{
			_context = context;
			_apiService = apiService;
		}

		[BindProperty]
		public FabricTypes FabricTypes { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null || _context.FabricTypes == null)
			{
				return NotFound();
			}

			var fabrictypes = await _context.FabricTypes.FirstOrDefaultAsync(m => m.ID == id);

			if (fabrictypes == null)
			{
				return NotFound();
			}
			else
			{
				FabricTypes = fabrictypes;
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null || _context.FabricTypes == null)
			{
				return NotFound();
			}

			bool deleted = await _apiService.DeleteItem<FabricTypes>(id.Value);
			if (!deleted)
			{
				return NotFound();
			}

			return RedirectToPage("./Index");
		}
	}
}
