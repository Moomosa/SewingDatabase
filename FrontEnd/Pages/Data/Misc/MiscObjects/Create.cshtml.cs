using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FrontEnd.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Misc.MiscObjects
{
	[Authorize(Roles = "User,Admin")]
	public class CreateModel : BaseCreateModel<SewingModels.Models.MiscObjects>
	{
		[BindProperty]
		public List<MiscItemType> MiscItemTypes { get; set; }

		public CreateModel(IHttpContextAccessor httpContextAccessor)
			: base(httpContextAccessor)
		{
		}

		public override async Task<IActionResult> OnGetAsync()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			MiscItemTypes = await ApiService.GatherAllRecords<MiscItemType>("MiscItemType", userId, 20);

			return await base.OnGetAsync();			
		}

		public override async Task<IActionResult> OnPostAsync()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			Item.ItemType = await ApiService.GetSingleItem<MiscItemType>(Item.ItemTypeID, userId);

			ModelState.Remove("Item.ItemType");

			return await base.OnPostAsync();
		}
	}
}
