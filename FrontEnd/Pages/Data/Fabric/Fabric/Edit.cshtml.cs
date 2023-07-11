using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json;

namespace FrontEnd.Pages.Data.Fabric.Fabric
{
	[Authorize(Roles = "User,Admin")]
	public class EditModel : PageModel
	{
		private readonly ApiService _apiService;

		public EditModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		[BindProperty]
		public SewingModels.Models.Fabric Fabric { get; set; } = default!;
		[BindProperty]
		public List<FabricBrand> FabricBrands { get; set; }
		[BindProperty]
		public List<FabricTypes> FabricTypes { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null || _apiService == null)
				return NotFound();

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var fabric = await _apiService.GetSingleItem<SewingModels.Models.Fabric>(id.Value, userId);
			if (fabric == null)
				return NotFound();

			Fabric = fabric;

			//Filling the lists with data from the DB according to the user
			FabricBrands = await _apiService.GetRecordsForUser<FabricBrand>("FabricBrand", userId);
			FabricTypes = await _apiService.GetRecordsForUser<FabricTypes>("FabricTypes", userId);

			//Setting the Lists to Session states for use in OnPost
			HttpContext.Session.SetString("BrandData", JsonConvert.SerializeObject(FabricBrands));
			HttpContext.Session.SetString("TypeData", JsonConvert.SerializeObject(FabricTypes));

			ViewData["FabricBrandID"] = new SelectList(FabricBrands, "ID", "FullName");
			ViewData["FabricTypeID"] = new SelectList(FabricTypes, "ID", "Type");
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

			if (!ModelState.IsValid)
				return Page();

			try
			{
				bool updated = await _apiService.UpdateItem<SewingModels.Models.Fabric>(Fabric.ID, Fabric);
				if (!updated)
					return NotFound();

				return RedirectToPage("./Index");
			}
			catch
			{
				return StatusCode(500, "An error occured while updating the item.");
			}
		}

	}
}
