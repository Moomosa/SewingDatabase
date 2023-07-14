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

namespace FrontEnd.Pages.Data.Elastic.Elastic
{
    [Authorize(Roles = "User,Admin")]
    public class DetailsModel : PageModel
    {
        private readonly ApiService _apiService;

        public DetailsModel(ApiService apiService)
        {
            _apiService = apiService;
        }

      public SewingModels.Models.Elastic Elastic { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _apiService == null)            
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var elastic = await _apiService.GetSingleItem<SewingModels.Models.Elastic>(id.Value, userId);
            if (elastic == null)            
                return NotFound();            
            else             
                Elastic = elastic;
            
            return Page();
        }
    }
}
