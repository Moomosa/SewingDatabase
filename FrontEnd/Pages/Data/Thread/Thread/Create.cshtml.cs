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

		public CreateModel(ApiService apiService, FrontHelpers frontHelpers, IHttpContextAccessor httpContextAccessor)
			: base(apiService, frontHelpers, httpContextAccessor)
		{
		}

		public override async Task<IActionResult> OnGetAsync()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			ThreadTypes = await _apiService.GatherAllRecords<ThreadTypes>("ThreadTypes", userId, 20);
			ThreadColors = await _apiService.GatherAllRecords<ThreadColor>("ThreadColor", userId, 20);
			ColorFamilies = await _apiService.GatherAllRecords<ThreadColorFamily>("ThreadColorFamily", userId, 20);

			return await base.OnGetAsync();
		}

		public override async Task<IActionResult> OnPostAsync()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			Item.ThreadType = await _apiService.GetSingleItem<ThreadTypes>(Item.ThreadTypeID, userId);
			Item.Color = await _apiService.GetSingleItem<ThreadColor>(Item.ColorID, userId);
			Item.ColorFamily = Item.Color.ColorFamily;
			Item.ColorFamilyID = Item.Color.ColorFamilyID;

			ModelState.Remove("Item.ThreadType");
			ModelState.Remove("Item.Color");
			ModelState.Remove("Item.ColorFamily");

			return await base.OnPostAsync();
		}
	}
}
