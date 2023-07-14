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

namespace FrontEnd.Pages.Data.Thread.Color
{
    [Authorize(Roles = "User,Admin")]
    public class DetailsModel : PageModel
    {
        private readonly ApiService _apiService;

        public DetailsModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public ThreadColor ThreadColor { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _apiService == null)            
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var threadcolor = await _apiService.GetSingleItem<ThreadColor>(id.Value, userId);
            if (threadcolor == null)            
                return NotFound();            
            else            
                ThreadColor = threadcolor;
            
            return Page();
        }
    }
}
