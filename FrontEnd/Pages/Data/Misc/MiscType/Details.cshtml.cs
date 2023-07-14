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
    public class DetailsModel : PageModel
    {
        private readonly ApiService _apiService;
        public DetailsModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public MiscItemType MiscItemType { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _apiService == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var miscitemtype = await _apiService.GetSingleItem<MiscItemType>(id.Value, userId);
            if (miscitemtype == null)
                return NotFound();
            else
                MiscItemType = miscitemtype;

            return Page();
        }
    }
}
