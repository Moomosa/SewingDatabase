using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models.Thread;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FrontEnd.Pages.Data.Thread.Family
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
        public ThreadColorFamily ThreadColorFamily { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ThreadColorFamily = await _apiService.GetSingleItem<ThreadColorFamily>(id.Value, userId);

            if (ThreadColorFamily == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || ThreadColorFamily == null)
                return NotFound();

            bool deleted = await _apiService.DeleteItem<ThreadColorFamily>(id.Value);
            if (!deleted)
            {
                TempData["DeleteFailureMessage"] = "Deletion is not allowed because it is used in a Thread.";
                return RedirectToPage("Delete", new { id });
            }

            return RedirectToPage("./Index");
        }
    }
}
