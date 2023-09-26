using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Machine
{
	[Authorize(Roles = "User,Admin")]
	public class DeleteModel : BaseDeleteModel<SewingModels.Models.Machine>
	{
		public DeleteModel(IHttpContextAccessor httpContextAccessor)
			: base(httpContextAccessor)
		{
		}
	}
}
