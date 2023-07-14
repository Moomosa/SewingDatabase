using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Misc.MiscType
{
    [Authorize(Roles = "User,Admin")]
    public class EditModel : PageModel
    {
        private readonly ApiService _apiService;

        public EditModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public MiscItemType MiscItemType { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _apiService == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var miscitemtype = await _apiService.GetSingleItem<MiscItemType>(id.Value, userId);
            if (miscitemtype == null)
                return NotFound();

            MiscItemType = miscitemtype;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)            
                return Page();

            try
            {
                bool updated = await _apiService.UpdateItem<MiscItemType>(MiscItemType.ID, MiscItemType);
                if (!updated)
                    return NotFound();

                return RedirectToPage("./Index");
            }
            catch
            {
                return StatusCode(500, "An error occured while updating the item.");
            }
        }
    }
}
