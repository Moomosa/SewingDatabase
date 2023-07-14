using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FrontEnd.Pages.Data.Elastic.Type
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
        public ElasticTypes ElasticTypes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ElasticTypes = await _apiService.GetSingleItem<ElasticTypes>(id.Value, userId);

            if (ElasticTypes == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || ElasticTypes == null)            
                return NotFound();

            bool deleted = await _apiService.DeleteItem<ElasticTypes>(id.Value);
            if (!deleted)
            {
                TempData["DeleteFailureMessage"] = "Deletion is not allowed because it is used in an Elastic.";
                return RedirectToPage("Delete", new { id });
            }

            return RedirectToPage("./Index");
        }
    }
}
