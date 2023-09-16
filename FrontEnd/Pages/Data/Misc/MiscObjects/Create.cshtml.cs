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
using Newtonsoft.Json;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Misc.MiscObjects
{
	[Authorize(Roles = "User,Admin")]
	public class CreateModel : PageModel
	{
		private readonly ApiService _apiService;

		[BindProperty]
		public SewingModels.Models.MiscObjects MiscObjects { get; set; } = default!;
		[BindProperty]
		public List<MiscItemType> MiscItemTypes { get; set; }
		public CreateModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public async Task<IActionResult> OnGet()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			MiscItemTypes = await _apiService.GetRecordsForUser<MiscItemType>("MiscItemType", userId);

			HttpContext.Session.SetString("MiscItemData", JsonConvert.SerializeObject(MiscItemTypes));

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var serializedItemTypes = HttpContext.Session.GetString("MiscItemData");
			MiscItemTypes = JsonConvert.DeserializeObject<List<MiscItemType>>(serializedItemTypes);

			MiscObjects.ItemType = MiscItemTypes.FirstOrDefault(mit => mit.ID == MiscObjects.ItemTypeID);

			ModelState.Remove("MiscObjects.ItemType");

			if (!ModelState.IsValid || _apiService == null)
				return Page();

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			HttpResponseMessage response = await _apiService.PostNewItem(MiscObjects, "/api/MiscObjects", userId);

			if (response.IsSuccessStatusCode)
			{
				HttpContext.Session.Remove("MObjects");
				HttpContext.Session.Remove("MOTotalRecords");
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
