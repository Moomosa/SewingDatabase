using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SewingModels.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FrontEnd.Pages.Data.Elastic.Type
{
	[Authorize(Roles = "User,Admin")]
	public class CreateModel : PageModel
	{
		private readonly ApiService _apiService;

		[BindProperty]
		public ElasticTypes ElasticTypes { get; set; } = default!;

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

			HttpResponseMessage response = await _apiService.PostNewItem(ElasticTypes, "/api/ElasticTypes", userId);

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
