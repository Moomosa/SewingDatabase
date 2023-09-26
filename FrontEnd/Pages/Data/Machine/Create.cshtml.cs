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
using Microsoft.AspNetCore.Mvc.Rendering;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Machine
{
	[Authorize(Roles = "User,Admin")]
	public class CreateModel : BaseCreateModel<SewingModels.Models.Machine>
	{
		public CreateModel(IHttpContextAccessor httpContextAccessor)
			: base(httpContextAccessor)
		{
		}

		public override async Task<IActionResult> OnPostAsync()
		{
			UpdateLastServiced();

			return await base.OnPostAsync();
		}

		public void UpdateLastServiced()
		{
			if(Item.LastServiced == null || Item.LastServiced == default)			
				Item.LastServiced = Item.PurchaseDate;			
		}
	}
}
