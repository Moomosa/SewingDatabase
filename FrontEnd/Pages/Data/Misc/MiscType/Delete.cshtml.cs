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

namespace FrontEnd.Pages.Data.Misc.MiscType
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
        public MiscItemType MiscItemType { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            MiscItemType = await _apiService.GetSingleItem<MiscItemType>(id.Value, userId);

            if (MiscItemType == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || MiscItemType == null)
                return NotFound();

            bool deleted = await _apiService.DeleteItem<MiscItemType>(id.Value);
            if (!deleted)
            {
                TempData["DeleteFailureMessage"] = "Deletion is not allowed because it is used in a Misc Object.";
                return RedirectToPage("Delete", new { id });
            }

            return RedirectToPage("./Index");
        }
    }
}
