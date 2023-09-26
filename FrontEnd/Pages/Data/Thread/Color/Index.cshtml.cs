using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Models.Thread;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Thread.Color
{
	[Authorize(Roles = "User,Admin")]
	public class IndexModel : BaseIndexModel<ThreadColor>
	{
        public IndexModel(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
			PagePath = "/Data/Thread/Color";
        }
	}
}
