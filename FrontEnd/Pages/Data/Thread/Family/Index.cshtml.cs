using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Models.Thread;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Thread.Family
{
	[Authorize(Roles = "User,Admin")]
	public class IndexModel : BaseIndexModel<ThreadColorFamily>
	{
        public IndexModel(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
			PagePath = "/Data/Thread/Family";
        }
	}
}
