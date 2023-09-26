using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;
using System.Security.Claims;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Fabric.Fabric
{
	[Authorize(Roles = "User,Admin")]
	public class DeleteModel : BaseDeleteModel<SewingModels.Models.Fabric>
	{
		public DeleteModel(IHttpContextAccessor httpContextAccessor)
			: base (httpContextAccessor)
		{			
		}
	}
}
