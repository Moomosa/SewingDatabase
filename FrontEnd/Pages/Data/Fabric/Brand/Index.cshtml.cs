using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace FrontEnd.Pages.Data.Fabric.Brand
{
	[Authorize(Roles = "User,Admin")]
	public class IndexModel : PageModel
	{
		private readonly ApiService _apiService;

		public IndexModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public IList<FabricBrand> FabricBrand { get; set; } = default!;

		public async Task OnGetAsync()
		{
			var userNameClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userNameClaim != null)
				FabricBrand = await _apiService.GetRecordsForUser<FabricBrand>("FabricBrand", userNameClaim);
		}
	}
}
