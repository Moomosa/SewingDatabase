using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Security.Claims;

namespace FrontEnd.Common
{
	[Authorize(Roles = "User,Admin")]
	public abstract class BaseDeleteModel<T> : PageModel where T : class
	{
		protected readonly string TypeName = typeof(T).Name;		
		protected readonly IHttpContextAccessor _httpContextAccessor;

		public BaseDeleteModel(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		[BindProperty]
		public T Item { get; set; } = default!;

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
			if (id == null || Item == null)
				return NotFound();

			bool deleted = await ApiService.DeleteItem<T>(id.Value);
			if (!deleted)
			{
				TempData["DeleteFailureMessage"] = "Deletion is not allowed.";
				return RedirectToPage("Delete", new { id });
			}

			FrontHelpers.ClearSessionData(TypeName, _httpContextAccessor);

			return RedirectToPage("./Index");
		}
	}
}
