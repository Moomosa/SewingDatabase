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
	public class IndexModel : PageModel
    {
        private readonly ApiService _apiService;

        public IndexModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IList<ThreadColor> ThreadColor { get;set; } = default!;

        public async Task OnGetAsync()
        {
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
                ThreadColor = await _apiService.GetRecordsForUser<ThreadColor>("ThreadColor", userId);
		}
	}
}
