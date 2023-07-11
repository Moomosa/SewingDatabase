using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BackendDatabase.Data;
using SewingModels.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace FrontEnd.Pages.Data.Fabric.Fabric
{
	[Authorize(Roles = "User,Admin")]
	public class CreateModel : PageModel
	{
		private readonly ApiService _apiService;

		[BindProperty]
		public SewingModels.Models.Fabric Fabric { get; set; }
		[BindProperty]
		public List<FabricBrand> FabricBrands { get; set; }
		[BindProperty]
		public List<FabricTypes> FabricTypes { get; set; }

		public CreateModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public async Task<IActionResult> OnGet()
		{
			var userNameClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

			//Filling the lists with data from the DB according to the user
			FabricBrands = await _apiService.GetRecordsForUser<FabricBrand>("FabricBrand", userNameClaim);
			FabricTypes = await _apiService.GetRecordsForUser<FabricTypes>("FabricTypes", userNameClaim);

			//Setting the Lists to Session states for use in OnPost
			HttpContext.Session.SetString("BrandData", JsonConvert.SerializeObject(FabricBrands));
			HttpContext.Session.SetString("TypeData", JsonConvert.SerializeObject(FabricTypes));

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			//This is pulling the Lists from Session states
			var serializedBrands = HttpContext.Session.GetString("BrandData");
			FabricBrands = JsonConvert.DeserializeObject<List<FabricBrand>>(serializedBrands);

			var serializedTypes = HttpContext.Session.GetString("TypeData");
			FabricTypes = JsonConvert.DeserializeObject<List<FabricTypes>>(serializedTypes);

			//So we can assign the values properly without another pull from DB
			Fabric.FabricBrand = FabricBrands.FirstOrDefault(fb => fb.ID == Fabric.FabricBrandID);
			Fabric.FabricType = FabricTypes.FirstOrDefault(ft => ft.ID == Fabric.FabricTypeID);

			//If we don't remove these, the ModelState becomes invalid even though properly configured
			ModelState.Remove("Fabric.FabricBrand");
			ModelState.Remove("Fabric.FabricType");

			if (!ModelState.IsValid || _apiService == null)
				return Page();

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			HttpResponseMessage response = await _apiService.PostNewItem(Fabric, "/api/Fabric", userId);

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
