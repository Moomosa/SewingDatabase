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
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Thread.Thread
{
	[Authorize(Roles = "User,Admin")]
	public class CreateModel : BaseCreateModel<SewingModels.Models.Thread>
	{
		[BindProperty]
		public List<ThreadTypes> ThreadTypes { get; set; }
		[BindProperty]
		public List<ThreadColor> ThreadColors { get; set; }
		[BindProperty]
		public List<ThreadColorFamily> ColorFamilies { get; set; }

		public CreateModel(IHttpContextAccessor httpContextAccessor)
			: base(httpContextAccessor)
		{
		}

		public override async Task<IActionResult> OnGetAsync()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			ThreadTypes = await ApiService.GatherAllRecords<ThreadTypes>("ThreadTypes", userId, 20);
			ThreadColors = await ApiService.GatherAllRecords<ThreadColor>("ThreadColor", userId, 20);
			ColorFamilies = await ApiService.GatherAllRecords<ThreadColorFamily>("ThreadColorFamily", userId, 20);

			return await base.OnGetAsync();
		}

		public override async Task<IActionResult> OnPostAsync()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			Item.ThreadType = await ApiService.GetSingleItem<ThreadTypes>(Item.ThreadTypeID, userId);
			Item.Color = await ApiService.GetSingleItem<ThreadColor>(Item.ColorID, userId);
			Item.ColorFamily = Item.Color.ColorFamily;
			Item.ColorFamilyID = Item.Color.ColorFamilyID;

			ModelState.Remove("Item.ThreadType");
			ModelState.Remove("Item.Color");
			ModelState.Remove("Item.ColorFamily");

			return await base.OnPostAsync();
		}
	}
}
