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
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Misc.MiscType
{
    [Authorize(Roles = "User,Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApiService _apiService;

        public CreateModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public MiscItemType MiscItemType { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _apiService == null)
                return Page();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            HttpResponseMessage response = await _apiService.PostNewItem(MiscItemType, "/api/MiscItemType", userId);

            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.Remove("MTypes");
                HttpContext.Session.Remove("MITypeTotalRecords");
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
