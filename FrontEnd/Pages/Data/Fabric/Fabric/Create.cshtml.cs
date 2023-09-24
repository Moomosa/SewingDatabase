using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SewingModels.Models;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Fabric.Item
{
	[Authorize(Roles = "User,Admin")]
	public class CreateModel : BaseCreateModel<SewingModels.Models.Fabric>
	{
		[BindProperty]
		public List<FabricBrand> FabricBrands { get; set; }
		[BindProperty]
		public List<FabricTypes> FabricTypes { get; set; }

		public CreateModel(ApiService apiService, FrontHelpers frontHelpers, IHttpContextAccessor httpContextAccessor)
			: base(apiService, frontHelpers, httpContextAccessor)
		{
		}

		public override async Task<IActionResult> OnGetAsync()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			FabricBrands = await _apiService.GatherAllRecords<FabricBrand>("FabricBrand", userId, 20);
			FabricTypes = await _apiService.GatherAllRecords<FabricTypes>("FabricTypes", userId, 20);

			return await base.OnGetAsync();			
		}

		public override async Task<IActionResult> OnPostAsync()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			Item.FabricBrand = await _apiService.GetSingleItem<FabricBrand>(Item.FabricBrandID, userId);
			Item.FabricType = await _apiService.GetSingleItem<FabricTypes>(Item.FabricTypeID, userId);

			//If we don't remove these, the ModelState becomes invalid even though properly configured
			ModelState.Remove("Item.FabricBrand");
			ModelState.Remove("Item.FabricType");

			return await base.OnPostAsync();
		}
	}
}
