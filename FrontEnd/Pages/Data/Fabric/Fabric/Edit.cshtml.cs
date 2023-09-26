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
using FrontEnd.Common;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace FrontEnd.Pages.Data.Fabric.Fabric
{
	[Authorize(Roles = "User,Admin")]
	public class EditModel : BaseEditModel<SewingModels.Models.Fabric>
	{
		public EditModel(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
		{
		}

		[BindProperty]
		public List<FabricBrand> FabricBrands { get; set; } = default!;
		[BindProperty]
		public List<FabricTypes> FabricTypes { get; set; } = default!;

		public override async Task<IActionResult> OnGetAsync(int? id)
		{
			await base.OnGetAsync(id);

			string userId = FrontHelpers.GetUserId(User);

			//Filling the lists with data from the DB according to the user
			FabricBrands = await ApiService.GatherAllRecords<FabricBrand>("FabricBrand", userId, 20);
			FabricTypes = await ApiService.GatherAllRecords<FabricTypes>("FabricTypes", userId, 20);

			ViewData["FabricBrandID"] = new SelectList(FabricBrands, "ID", "FullName");
			ViewData["FabricTypeID"] = new SelectList(FabricTypes, "ID", "Type");
			return Page();
		}

		public override async Task<IActionResult> OnPostAsync(int? id)
		{
			string userId = FrontHelpers.GetUserId(User);

            Item.FabricBrand = await ApiService.GetSingleItem<FabricBrand>(Item.FabricBrandID, userId);
            Item.FabricType = await ApiService.GetSingleItem<FabricTypes>(Item.FabricTypeID, userId);

            //If we don't remove these, the ModelState becomes invalid even though properly configured
            ModelState.Remove("Item.FabricBrand");
			ModelState.Remove("Item.FabricType");

			return await base.OnPostAsync(id);
		}
	}
}
