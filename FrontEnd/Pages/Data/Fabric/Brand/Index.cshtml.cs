using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;
using Microsoft.AspNetCore.Identity;

namespace FrontEnd.Pages.Data.Fabric.Brand
{
	public class IndexModel : PageModel
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly ApiService _apiService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public IndexModel(UserManager<IdentityUser> userManager, ApiService apiService, IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_apiService = apiService;
			_httpContextAccessor = httpContextAccessor;
		}

		public IList<FabricBrand> FabricBrand { get; set; } = default!;

		public async Task OnGetAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user != null)
				FabricBrand = await _apiService.GetRecordsForUser<FabricBrand>("FabricBrand", user.UserName);

		}
	}
}
