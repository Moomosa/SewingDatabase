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

namespace FrontEnd.Pages.Data.Machine
{
	[Authorize(Roles = "User,Admin")]
	public class CreateModel : PageModel
	{
		private readonly ApiService _apiService;

		[BindProperty]
		public SewingModels.Models.Machine Machine { get; set; } = default!;

		public CreateModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public IActionResult OnGet()
		{
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid || _apiService == null)
				return Page();

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			HttpResponseMessage response = await _apiService.PostNewItem(Machine, "/api/Machine", userId);

			if (response.IsSuccessStatusCode)
				return RedirectToPage("./Index");
			else
			{
				ModelState.AddModelError("", "Failed to create item");
				return Page();
			}
		}
	}
}
