using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Security.Claims;

namespace FrontEnd.Common
{
	[Authorize(Roles = "User,Admin")]
	public abstract class BaseCreateModel<T> : PageModel where T : class
	{
		protected readonly string TypeName = typeof(T).Name;
		protected readonly IHttpContextAccessor _httpContextAccessor;

		[BindProperty]
		public T Item { get; set; } = default!;

		protected BaseCreateModel(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public virtual async Task<IActionResult> OnGetAsync()
		{
			return Page();
		}

		public virtual async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();

			string userId = FrontHelpers.GetUserId(User);

			HttpResponseMessage response = await ApiService.PostNewItem(Item, $"/api/{TypeName}", userId);

			if (response.IsSuccessStatusCode)
			{
				FrontHelpers.ClearSessionData(TypeName, _httpContextAccessor);
				return RedirectToPage("./Index");
			}
			else
			{
				ModelState.AddModelError("", "Failed to create item");
				return Page();
			}
		}
	}
}
