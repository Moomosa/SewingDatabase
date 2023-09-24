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
using System.Security.Claims;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Elastic.Type
{
	[Authorize(Roles = "User,Admin")]
	public class CreateModel : BaseCreateModel<ElasticTypes>
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
