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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Misc.MiscObjects
{
	[Authorize(Roles = "User,Admin")]
	public class EditModel : BaseEditModel<SewingModels.Models.MiscObjects>
	{
		public EditModel(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
		{
		}

		[BindProperty]
		public List<MiscItemType> MiscItemTypes { get; set; } = default!;

		public override async Task<IActionResult> OnGetAsync(int? id)
		{
			await base.OnGetAsync(id);

			string userId = FrontHelpers.GetUserId(User);

			MiscItemTypes = await ApiService.GatherAllRecords<MiscItemType>("MiscItemType", userId, 20);

			ViewData["ItemTypeID"] = new SelectList(MiscItemTypes, "ID", "Item");
			return Page();
		}

		public override async Task<IActionResult> OnPostAsync(int? id)
		{
			string userId = FrontHelpers.GetUserId(User);

			Item.ItemType = await ApiService.GetSingleItem<MiscItemType>(Item.ItemTypeID, userId);

			ModelState.Remove("Item.ItemType");

			return await base.OnPostAsync(id);
		}
	}
}
