using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SewingModels.Models;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Elastic.Elastic
{
	[Authorize(Roles = "User,Admin")]
	public class IndexModel : BaseIndexModel<SewingModels.Models.Elastic>
	{
        public IndexModel(IHttpContextAccessor httpContextAccessor)
    : base(httpContextAccessor)
        {
			PagePath = "/Data/Elastic/Elastic";
        }
	}
}
