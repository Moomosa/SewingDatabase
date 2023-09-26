using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SewingModels.Models;
using System.Security.Permissions;

namespace FrontEnd.Common
{
	public class BaseEditModel<T> : PageModel where T : class
	{
		protected readonly string TypeName = typeof(T).Name;
		protected readonly IHttpContextAccessor _httpContextAccessor;

		[BindProperty]
		public T Item { get; set; } = default!;

		public BaseEditModel(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public virtual async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			string userId = FrontHelpers.GetUserId(User);

			Item = await ApiService.GetSingleItem<T>(id.Value, userId);
			if (Item == null)
				return NotFound();

			return Page();
		}

		public virtual async Task<IActionResult> OnPostAsync(int? id)
		{
			if (!ModelState.IsValid)
				return Page();

			try
			{
				if (!id.HasValue)
					return NotFound();

				bool updated = await ApiService.UpdateItem<T>(id.Value, Item);
				if (!updated)
					return NotFound();

				FrontHelpers.ClearSessionData(TypeName, _httpContextAccessor);

				return RedirectToPage("./Index");
			}
			catch
			{
				return StatusCode(500, "An error occurred while updating the item.");
			}
		}
	}
}
