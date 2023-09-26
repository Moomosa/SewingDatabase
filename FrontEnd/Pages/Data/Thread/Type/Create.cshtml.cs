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

namespace FrontEnd.Pages.Data.Thread.Type
{
	[Authorize(Roles = "User,Admin")]
	public class CreateModel : BaseCreateModel<ThreadTypes>
	{
		public CreateModel(IHttpContextAccessor httpContextAccessor)
			: base(httpContextAccessor)
		{
		}
	}
}
