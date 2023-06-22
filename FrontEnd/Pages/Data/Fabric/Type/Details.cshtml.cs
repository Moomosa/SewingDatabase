using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Fabric.Type
{
	public class DetailsModel : PageModel
	{
		private readonly ApiService _apiService;
		public DetailsModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public FabricTypes FabricTypes { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null || _apiService == null)
				return NotFound();

			var fabrictypes = await _apiService.GetSingleItem<FabricTypes>(id.Value);
			if (fabrictypes == null)
			{
				return NotFound();
			}
			else
			{
				FabricTypes = fabrictypes;
			}
			return Page();
		}
	}
}
