using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;
using System.Security.Claims;
using Microsoft.OpenApi.Exceptions;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace FrontEnd.Pages.Data.Fabric.Type
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
        public FabricTypes FabricTypes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			FabricTypes = await _apiService.GetSingleItem<FabricTypes>(id.Value,userId);

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
            {
                TempData["DeleteFailureMessage"] = "Deletion is not allowed because it is used in a Fabric.";
                return RedirectToPage("Delete", new { id });
            }

            return RedirectToPage("./Index");
        }
    }
}
