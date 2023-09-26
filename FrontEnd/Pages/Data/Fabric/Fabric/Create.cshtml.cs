using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SewingModels.Models;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Fabric.Fabric
{
	[Authorize(Roles = "User,Admin")]
	public class CreateModel : BaseCreateModel<SewingModels.Models.Fabric>
	{
		[BindProperty]
		public List<FabricBrand> FabricBrands { get; set; } = default!;
		[BindProperty]
		public List<FabricTypes> FabricTypes { get; set; } = default!;

		public CreateModel(IHttpContextAccessor httpContextAccessor)
			: base(httpContextAccessor)
		{
		}

		public override async Task<IActionResult> OnGetAsync()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			FabricBrands = await ApiService.GatherAllRecords<FabricBrand>("FabricBrand", userId, 20);
			FabricTypes = await ApiService.GatherAllRecords<FabricTypes>("FabricTypes", userId, 20);

			return await base.OnGetAsync();			
		}

		public override async Task<IActionResult> OnPostAsync()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			Item.FabricBrand = await ApiService.GetSingleItem<FabricBrand>(Item.FabricBrandID, userId);
			Item.FabricType = await ApiService.GetSingleItem<FabricTypes>(Item.FabricTypeID, userId);

			//If we don't remove these, the ModelState becomes invalid even though properly configured
			ModelState.Remove("Item.FabricBrand");
			ModelState.Remove("Item.FabricType");

			return await base.OnPostAsync();
		}
	}
}
