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
			UserId = FrontHelpers.GetUserId(User);
			if (UserId == null) return RedirectToPage("/Account/Login");

			PageSize = FrontHelpers.GetCurrentPageSize(TypeName, _httpContextAccessor);
			TotalRecords = await FrontHelpers.GetTotalRecords<T>(TypeName, UserId, _httpContextAccessor);
			LastPageVisited = FrontHelpers.GetLastPageVisited(TypeName, _httpContextAccessor);


			string referrer = HttpContext.Request.Headers["Referer"];
			if (!string.IsNullOrEmpty(referrer) && referrer.Contains(PagePath))
			{
				if (CurrentPage != LastPageVisited) // Handle changing pagination page				
					Items = await FrontHelpers.CallForRecords<T>(TypeName, UserId, CurrentPage, PageSize, _httpContextAccessor);
				else                                // Handle refresh possibility				
					Items = FrontHelpers.GetRecordsFromSession<T>(TypeName, _httpContextAccessor);
			}
			else
			{
				if (LastPageVisited != 0)           // Handle coming from not this page
				{
					CurrentPage = LastPageVisited;
					Items = FrontHelpers.GetRecordsFromSession<T>(TypeName, _httpContextAccessor);
				}
				else                                // Handle the first time coming to the page				
					Items = await FrontHelpers.CallForRecords<T>(TypeName, UserId, CurrentPage, PageSize, _httpContextAccessor);
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

			FrontHelpers.SetPageValues(TypeName, 0, selectedPageSize, _httpContextAccessor);

			return RedirectToPage("./Index", new { currentPage = CurrentPage });
		}
	}
}
