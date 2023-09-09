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
using ModelLibrary.Models.Thread;
using System.Security.Claims;
using Newtonsoft.Json;

namespace FrontEnd.Pages.Data.Thread.Thread
{
	[Authorize(Roles = "User,Admin")]
	public class CreateModel : PageModel
	{
		private readonly ApiService _apiService;

		[BindProperty]
		public SewingModels.Models.Thread Thread { get; set; } = default!;
		[BindProperty]
		public List<ThreadTypes> ThreadTypes { get; set; }
		[BindProperty]
		public List<ThreadColor> ThreadColors { get; set; }
		[BindProperty]
		public List<ThreadColorFamily> ColorFamilies { get; set; }

		public CreateModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public async Task<IActionResult> OnGet()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			ThreadTypes = await _apiService.GetRecordsForUser<ThreadTypes>("ThreadTypes", userId);
			ThreadColors = await _apiService.GetRecordsForUser<ThreadColor>("ThreadColor", userId);
			ColorFamilies = await _apiService.GetRecordsForUser<ThreadColorFamily>("ThreadColorFamily", userId);

			HttpContext.Session.SetString("ThreadTypeData", JsonConvert.SerializeObject(ThreadTypes));
			HttpContext.Session.SetString("ThreadColorData", JsonConvert.SerializeObject(ThreadColors));
			HttpContext.Session.SetString("ThreadFamilyData", JsonConvert.SerializeObject(ColorFamilies));

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var serializedTypes = HttpContext.Session.GetString("ThreadTypeData");
			ThreadTypes = JsonConvert.DeserializeObject<List<ThreadTypes>>(serializedTypes);

			var serializedColors = HttpContext.Session.GetString("ThreadColorData");
			ThreadColors = JsonConvert.DeserializeObject<List<ThreadColor>>(serializedColors);

			var serializedFamilies = HttpContext.Session.GetString("ThreadFamilyData");
			ColorFamilies = JsonConvert.DeserializeObject<List<ThreadColorFamily>>(serializedFamilies);

			Thread.ThreadType = ThreadTypes.FirstOrDefault(tt => tt.ID == Thread.ThreadTypeID);
			Thread.Color = ThreadColors.FirstOrDefault(tc => tc.ID == Thread.ColorID);
			Thread.ColorFamily = ColorFamilies.FirstOrDefault(cf => cf.ID == Thread.Color.ColorFamilyID);
			Thread.ColorFamilyID = Thread.Color.ColorFamilyID;

			ModelState.Remove("Thread.ThreadType");
			ModelState.Remove("Thread.Color");
			ModelState.Remove("Thread.ColorFamily");

			if (!ModelState.IsValid || _apiService == null)
				return Page();

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			HttpResponseMessage response = await _apiService.PostNewItem(Thread, "/api/Thread", userId);

			if (response.IsSuccessStatusCode)
			{
				HttpContext.Session.Remove("Threads");
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
