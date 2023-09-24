using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace FrontEnd.Common
{
	public abstract class BaseCreateModel<T> : PageModel where T : class
	{
		protected readonly ApiService _apiService;
		protected readonly FrontHelpers _frontHelpers;
		protected readonly IHttpContextAccessor _httpContextAccessor;
		protected readonly string TypeName = typeof(T).Name;

		[BindProperty]
		public T Item { get; set; }

		protected BaseCreateModel(ApiService apiService, FrontHelpers frontHelpers, IHttpContextAccessor httpContextAccessor)
		{
			_apiService = apiService;
			_frontHelpers = frontHelpers;
			_httpContextAccessor = httpContextAccessor;
		}

		public virtual async Task<IActionResult> OnGetAsync()
		{
			return Page();
		}

		public virtual async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid || _apiService == null)
				return Page();

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			HttpResponseMessage response = await _apiService.PostNewItem(Item, $"/api/{TypeName}", userId);

			if (response.IsSuccessStatusCode)
			{
				_frontHelpers.ClearSessionData(TypeName, _httpContextAccessor);
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
