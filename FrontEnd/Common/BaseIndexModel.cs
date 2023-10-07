using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Security.Claims;

namespace FrontEnd.Common
{
	[Authorize(Roles = "User,Admin")]
	public abstract class BaseIndexModel<T> : PageModel where T : class
	{
		protected readonly string TypeName = typeof(T).Name;
		protected readonly IHttpContextAccessor _httpContextAccessor;

		public BaseIndexModel(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public IList<T> Items { get; set; } = default!;
		protected string UserId { get; private set; }
		public string PagePath { get; set; }
		#region Paging Properties
		[BindProperty(SupportsGet = true)]
		public int CurrentPage { get; set; }
		protected int LastPageVisited { get; set; }
		public int PageSize { get; set; }
		public List<int> PageSizes { get; } = new() { 5, 10, 20, 50 };
		public int TotalRecords { get; set; } = -1;
		public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalRecords, PageSize));
		public bool ShowPrevious => CurrentPage > 1;
		public bool ShowNext => CurrentPage < TotalPages;
		#endregion
		#region Sort Properties
		[BindProperty(SupportsGet = true)]
		public string Sort { get; set; }
		[BindProperty(SupportsGet = true)]
		public string SortDirection { get; set; }
		protected string SortedAs { get; set; }
		#endregion

		public virtual async Task<IActionResult> OnGetAsync()
		{
			#region Set Properties
			UserId = FrontHelpers.GetUserId(User);
			if (UserId == null) return RedirectToPage("/Account/Login");

			PageSize = FrontHelpers.GetCurrentPageSize(TypeName, _httpContextAccessor);
			TotalRecords = await FrontHelpers.GetTotalRecords<T>(TypeName, UserId, _httpContextAccessor);
			LastPageVisited = FrontHelpers.GetLastPageVisited(TypeName, _httpContextAccessor);

			SortedAs = FrontHelpers.GetSortBy(TypeName, _httpContextAccessor);
			#endregion

			string referrer = HttpContext.Request.Headers["Referer"];
			if (!string.IsNullOrEmpty(referrer) && referrer.Contains(PagePath))
			{
				if (CurrentPage != LastPageVisited || Sort != SortedAs) // Handle changing pagination page				
					Items = await FrontHelpers.CallForRecords<T>(TypeName, UserId, CurrentPage, PageSize, Sort, SortDirection, _httpContextAccessor);
				else                                // Handle refresh possibility				
					Items = FrontHelpers.GetRecordsFromSession<T>(TypeName, _httpContextAccessor);
			}
			else
			{
				if (LastPageVisited != 0 || Sort != null && Sort != SortedAs)           // Handle coming from not this page
				{
					CurrentPage = LastPageVisited;
					Items = FrontHelpers.GetRecordsFromSession<T>(TypeName, _httpContextAccessor);
				}
				else                                // Handle the first time coming to the page				
					Items = await FrontHelpers.CallForRecords<T>(TypeName, UserId, CurrentPage, PageSize, Sort, SortDirection, _httpContextAccessor);
			}

			return Page();
		}

		public virtual async Task<IActionResult> OnPost(int selectedPageSize)
		{
			UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			int totalRecords = await FrontHelpers.GetTotalRecords<T>(TypeName, UserId, _httpContextAccessor);
			int totalPages = (int)Math.Ceiling((decimal)totalRecords / selectedPageSize);

			if (CurrentPage > totalPages)
				CurrentPage = totalPages;
			else if (CurrentPage < 1)
				CurrentPage = 1;

			FrontHelpers.SetPageValues(TypeName, 0, selectedPageSize, Sort, _httpContextAccessor);

			return RedirectToPage("./Index", new { currentPage = CurrentPage, sort = Sort, sortDirection = SortDirection });
		}

		public IActionResult OnGetSort(string sort)
		{
			return RedirectToPage("./Index", new { CurrentPage = 1, sort, SortDirection });
		}
	}
}
