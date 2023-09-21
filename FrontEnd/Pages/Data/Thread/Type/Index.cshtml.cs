using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SewingModels.Models;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Thread.Type
{
	[Authorize(Roles = "User,Admin")]
	public class IndexModel : BaseIndexModel<ThreadTypes>
	{
        public IndexModel(ApiService apiService, FrontHelpers frontHelpers, IHttpContextAccessor httpContextAccessor)
            : base(apiService, frontHelpers, httpContextAccessor)
        {
			PagePath = "/Data/Thread/Type";
        }

        public override async Task<IActionResult> OnGetAsync()
		{
			await base.OnGetAsync();

            return Page();
		}
	}
}
