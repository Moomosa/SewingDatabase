using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;
using System.Security.Claims;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace FrontEnd.Pages.Data.Fabric.Fabric
{
	[Authorize(Roles = "User,Admin")]
	public class IndexModel : PageModel
    {
        private readonly ApiService _apiService;

        public IndexModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IList<SewingModels.Models.Fabric> Fabric { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var userNameClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userNameClaim != null)
                Fabric = await _apiService.GetRecordsForUser<SewingModels.Models.Fabric>("Fabric", userNameClaim);
        }
    }
}
