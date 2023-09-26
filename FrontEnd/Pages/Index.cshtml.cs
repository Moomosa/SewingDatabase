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

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public MultiDataDTO multiData { get; set; }

		public async Task OnGet()
		{
			if (User.IsInRole("User"))
			{
				string userId = FrontHelpers.GetUserId(User);
				//var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);				
				multiData = await ApiService.GetMultiData(userId, 5);
			}
		}
	}
}