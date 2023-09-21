using System;
using System.Collections.Generic;
using System.Linq;
using FrontEnd.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Thread.Thread
{
	[Authorize(Roles = "User,Admin")]
	public class IndexModel : BaseIndexModel<SewingModels.Models.Thread>
	{
        public IndexModel(ApiService apiService, FrontHelpers frontHelpers, IHttpContextAccessor httpContextAccessor)
            : base(apiService, frontHelpers, httpContextAccessor)
        {
			PagePath = "/Data/Thread/Thread";
        }

        public override async Task<IActionResult> OnGetAsync()
		{
			await base.OnGetAsync();

            return Page();
		}
	}
}
