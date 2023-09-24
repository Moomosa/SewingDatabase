using System;
using Microsoft.AspNetCore.Mvc;
using SewingModels.Models;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Fabric.Type
{
	[Authorize(Roles = "User,Admin")]
	public class CreateModel : BaseCreateModel<FabricTypes>
	{
		public CreateModel(ApiService apiService, FrontHelpers frontHelpers, IHttpContextAccessor httpContextAccessor)
			: base(apiService, frontHelpers, httpContextAccessor)
		{
		}

		public override async Task<IActionResult> OnGetAsync()
		{
			await base.OnGetAsync();
			return Page();
		}

		public override async Task<IActionResult> OnPostAsync()
		{
			return await base.OnPostAsync();
		}
	}
}
