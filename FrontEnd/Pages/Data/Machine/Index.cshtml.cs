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

namespace FrontEnd.Pages.Data.Machine
{
	[Authorize(Roles = "User,Admin")]
	public class IndexModel : PageModel
	{
		private readonly ApiService _apiService;

		public IndexModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public IList<SewingModels.Models.Machine> Machine { get; set; } = default!;

		public async Task OnGetAsync()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId != null)
				Machine = await _apiService.GetRecordsForUser<SewingModels.Models.Machine>("Machine", userId);
		}
	}
}
