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

namespace FrontEnd.Pages.Data.Fabric.Type
{
	public class IndexModel : PageModel
	{
		private readonly ApiService _apiService;

		public IndexModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public IList<FabricTypes> FabricTypes { get; set; } = default!;

		public async Task OnGetAsync()
		{
			var userNameClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userNameClaim != null)
				FabricTypes = await _apiService.GetRecordsForUser<FabricTypes>("FabricTypes", userNameClaim);
		}
	}
}
