using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Elastic.Elastic
{
	[Authorize(Roles = "User,Admin")]
	public class EditModel : BaseEditModel<SewingModels.Models.Elastic>
	{
		[BindProperty]
		public List<ElasticTypes> ElasticTypes { get; set; } = default!;

		public EditModel(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
		{
		}

		public override async Task<IActionResult> OnGetAsync(int? id)
		{
			await base.OnGetAsync(id);

			string userId = FrontHelpers.GetUserId(User);

			ElasticTypes = await ApiService.GetRecordsForUser<ElasticTypes>("ElasticTypes", userId);

			ViewData["ElasticTypeID"] = new SelectList(ElasticTypes, "ID", "Type");
			return Page();
		}

		public override async Task<IActionResult> OnPostAsync(int? id)
		{
			string userId = FrontHelpers.GetUserId(User);

			Item.ElasticType = await ApiService.GetSingleItem<ElasticTypes>(Item.ElasticTypeID, userId);

			ModelState.Remove("Item.ElasticType");

			return await base.OnPostAsync(id);
		}
	}
}
