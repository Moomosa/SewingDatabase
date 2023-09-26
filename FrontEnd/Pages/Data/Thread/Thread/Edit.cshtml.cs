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
using ModelLibrary.Models.Thread;
using Newtonsoft.Json;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Thread.Thread
{
	[Authorize(Roles = "User,Admin")]
	public class EditModel : BaseEditModel<SewingModels.Models.Thread>
	{
		public EditModel(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
		{
		}

		[BindProperty]
		public List<ThreadTypes> ThreadTypes { get; set; } = default!;
		[BindProperty]
		public List<ThreadColor> ThreadColors { get; set; } = default!;
		[BindProperty]
		public List<ThreadColorFamily> ColorFamilies { get; set; } = default!;

		public override async Task<IActionResult> OnGetAsync(int? id)
		{
			await base.OnGetAsync(id);

			string userId = FrontHelpers.GetUserId(User);

			ThreadTypes = await ApiService.GatherAllRecords<ThreadTypes>("ThreadTypes", userId, 20);
			ThreadColors = await ApiService.GatherAllRecords<ThreadColor>("ThreadColor", userId, 20);
			ColorFamilies = await ApiService.GatherAllRecords<ThreadColorFamily>("ThreadColorFamily", userId, 20);

			ViewData["ThreadTypeID"] = new SelectList(ThreadTypes, "ID", "Name");
			ViewData["ColorID"] = new SelectList(ThreadColors, "ID", "Color");
			ViewData["ColorFamilyID"] = new SelectList(ColorFamilies, "ID", "ColorFamily");
			return Page();
		}

		public override async Task<IActionResult> OnPostAsync(int? id)
		{
			string userId = FrontHelpers.GetUserId(User);

			Item.ThreadType = await ApiService.GetSingleItem<ThreadTypes>(Item.ThreadTypeID, userId);
			Item.Color = await ApiService.GetSingleItem<ThreadColor>(Item.ColorID, userId);
			Item.ColorFamily = await ApiService.GetSingleItem<ThreadColorFamily>(Item.Color.ColorFamilyID, userId);
			Item.ColorFamilyID = Item.Color.ColorFamilyID;

			ModelState.Remove("Item.ThreadType");
			ModelState.Remove("Item.Color");
			ModelState.Remove("Item.ColorFamily");

			return await base.OnPostAsync(id);
		}
	}
}
