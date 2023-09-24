using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BackendDatabase.Data;
using ModelLibrary.Models.Thread;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Thread.Family
{
    [Authorize(Roles = "User,Admin")]
    public class CreateModel : BaseCreateModel<ThreadColorFamily>
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
