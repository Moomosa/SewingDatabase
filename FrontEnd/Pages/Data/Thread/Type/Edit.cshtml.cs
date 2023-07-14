using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FrontEnd.Pages.Data.Thread.Type
{
    [Authorize(Roles = "User,Admin")]
    public class EditModel : PageModel
    {
        private readonly ApiService _apiService;

        [BindProperty]
        public ThreadTypes ThreadTypes { get; set; } = default!;

        public EditModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _apiService == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var threadtypes = await _apiService.GetSingleItem<ThreadTypes>(id.Value, userId);
            if (threadtypes == null)
                return NotFound();

            ThreadTypes = threadtypes;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            try
            {
                bool updated = await _apiService.UpdateItem<ThreadTypes>(ThreadTypes.ID, ThreadTypes);
                if (!updated)
                    return Page();

                return RedirectToPage("./Index");
            }
            catch
            {
                return StatusCode(500, "An error occured while updating the item.");
            }
        }
    }
}
