using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FrontEnd.Pages.Data.Elastic.Elastic
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
        public SewingModels.Models.Elastic Elastic { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)            
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Elastic = await _apiService.GetSingleItem<SewingModels.Models.Elastic>(id.Value, userId);            

            if (Elastic == null)            
                return NotFound();            
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || Elastic == null)            
                return NotFound();

            bool deleted = await _apiService.DeleteItem<SewingModels.Models.Elastic>(id.Value);
            if (!deleted)
                return NotFound();

            return RedirectToPage("./Index");
        }
    }
}
