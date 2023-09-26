using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SewingModels.Models;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Fabric.Brand
{
	[Authorize(Roles = "User,Admin")]
	public class CreateModel : BaseCreateModel<FabricBrand>
	{
		public CreateModel(IHttpContextAccessor httpContextAccessor)
			: base(httpContextAccessor)
		{
		}
	}
}
