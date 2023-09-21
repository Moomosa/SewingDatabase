using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace FrontEnd.Common
{
	public abstract class BaseIndexModel<T> : PageModel where T : class
	{
		protected readonly string TypeName = typeof(T).Name;
		protected readonly ApiService _apiService;
		protected readonly FrontHelpers _frontHelpers;
		protected readonly IHttpContextAccessor _httpContextAccessor;

		public BaseIndexModel(ApiService apiService, FrontHelpers frontHelpers, IHttpContextAccessor httpContextAccessor)
		{
			_apiService = apiService;
			_frontHelpers = frontHelpers;
			_httpContextAccessor = httpContextAccessor;
		}

		public IList<T> Items { get; set; } = default!;
		[BindProperty(SupportsGet = true)]
		public int CurrentPage { get; set; } = 1;
		public int PageSize { get; set; }
		public List<int> PageSizes { get; } = new() { 5, 10, 20, 50 };
		public int TotalRecords { get; set; } = -1;
		public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalRecords, PageSize));
		public bool ShowPrevious => CurrentPage > 1;
		public bool ShowNext => CurrentPage < TotalPages;
		public bool ShowFirst => CurrentPage != 1;
		public bool ShowLast => CurrentPage != TotalPages;
		protected int LastPageVisited { get; set; }
		protected string UserId { get; private set; }
		public string PagePath { get; set; }

		public virtual async Task<IActionResult> OnGetAsync()
		{
			UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (UserId == null) return RedirectToPage("/Account/Login");

			TotalRecords = await _frontHelpers.GetTotalRecords<T>(TypeName, UserId, _httpContextAccessor, _apiService);
			PageSize = _frontHelpers.GetCurrentPageSize(TypeName, _httpContextAccessor);
			LastPageVisited = _frontHelpers.GetLastPageVisited(TypeName, _httpContextAccessor);

			string referrer = HttpContext.Request.Headers["Referer"];
			if (!string.IsNullOrEmpty(referrer) && referrer.Contains(PagePath))
			{
				if (CurrentPage != LastPageVisited) // Handle changing pagination page				
					Items = await _frontHelpers.CallForRecords<T>(TypeName, UserId, CurrentPage, PageSize, _httpContextAccessor, _apiService);
				else // Handle refresh possibility				
					Items = _frontHelpers.GetRecordsFromSession<T>(TypeName, _httpContextAccessor);
			}
			else
			{
				if (LastPageVisited != 0) // Handle coming from not this page
				{
					CurrentPage = LastPageVisited;
					Items = _frontHelpers.GetRecordsFromSession<T>(TypeName, _httpContextAccessor);
				}
				else // Handle the first time coming to the page				
					Items = await _frontHelpers.CallForRecords<T>(TypeName, UserId, CurrentPage, PageSize, _httpContextAccessor, _apiService);
			}

			return Page();
		}

		public async Task<IActionResult> OnPost(int selectedPageSize)
		{
			_frontHelpers.SetPageValues(TypeName, 0, selectedPageSize, _httpContextAccessor);
			return RedirectToPage();
		}
	}
}
