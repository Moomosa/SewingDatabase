using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLibrary.Models.Database;
using ModelLibrary.Models.Database.DTO;
using System.Security.Claims;

namespace FrontEnd.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly ApiService _apiService;

		public IndexModel(ILogger<IndexModel> logger, ApiService apiService)
		{
			_logger = logger;
			_apiService = apiService;
		}

		public MultiDataDTO multiData { get; set; }

		public async Task OnGet()
		{
			if (User.IsInRole("User"))
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				multiData = await _apiService.GetMultiData(userId, 5);
			}
		}
	}
}