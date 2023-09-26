using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FrontEnd.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Thread.Type
{
	[Authorize(Roles = "User,Admin")]
	public class DeleteModel : BaseDeleteModel<ThreadTypes>
	{
		public DeleteModel(IHttpContextAccessor httpContextAccessor)
			: base(httpContextAccessor)
		{
		}
	}
}
