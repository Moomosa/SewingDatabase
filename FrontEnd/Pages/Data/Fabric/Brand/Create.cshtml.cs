using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SewingModels.Models;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Fabric.Brand
{
	[Authorize(Roles = "User,Admin")]
	public class CreateModel : BaseCreateModel<FabricBrand>
	{
		public CreateModel(ApiService apiService, FrontHelpers frontHelpers, IHttpContextAccessor httpContextAccessor)
			: base(apiService, frontHelpers, httpContextAccessor)
		{
		}

		public override async Task<IActionResult> OnGetAsync()
		{
			return await base.OnGetAsync();
		}

		public override async Task<IActionResult> OnPostAsync()
		{
			return await base.OnPostAsync();
		}
	}
}
