using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Security.Claims;

namespace FrontEnd.Common
{
	[Authorize(Roles = "User,Admin")]
	public abstract class BaseDetailsModel<T> : PageModel where T:class
	{
		public BaseDetailsModel()
		{
		}

		public T Item { get; set; } = default!;

		public virtual async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			string userId = FrontHelpers.GetUserId(User);

			var item = await ApiService.GetSingleItem<T>(id.Value, userId);
			if (item == null)
				return NotFound();
			else
				Item = item;

			return Page();
		}
	}
}
