using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLibrary.Models.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net.NetworkInformation;
using System.Reflection;
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
        #region Color Properties
        public Dictionary<string, string> colorAssignments = new();
        public List<string> predefinedColors = new List<string>()
        {
            "#0d6efd",	//Blue
			"#fd7e14",	//Orange
			"#6f42c1",	//Purple
			"198754",	//Green
			"#d63384",	//Pink
			"#dc3545",	//Red
			"#20c997",	//Teal
			"#6610f2",	//Indigo
			"#ffc107",	//Yellow
			"#0dcaf0",	//Cyan
			"#fff",		//White
			"#6c757d",	//Gray
			"#343a40",	//Dark Gray
			"#000000"	//Black
		};
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
                // Handle changing pagination page				
                if (CurrentPage != LastPageVisited || Sort != SortedAs)
                    Items = await FrontHelpers.CallForRecords<T>(TypeName, UserId, CurrentPage, PageSize, Sort, SortDirection, _httpContextAccessor);
                else  // Handle refresh possibility				
                    Items = FrontHelpers.GetRecordsFromSession<T>(TypeName, _httpContextAccessor);
            }
            else
            {
                // Handle coming from not this page
                if (LastPageVisited != 0 || Sort != null && Sort != SortedAs)
                {
                    CurrentPage = LastPageVisited;
                    Items = FrontHelpers.GetRecordsFromSession<T>(TypeName, _httpContextAccessor);
                }
                else  // Handle the first time coming to the page				
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

        public string GetBackgroundColor(string type)
        {
            if (colorAssignments.ContainsKey(type))
                return colorAssignments[type];
            else
            {
                var backgroundColor = predefinedColors[colorAssignments.Count % predefinedColors.Count];
                colorAssignments[type] = backgroundColor;
                return backgroundColor;
            }
        }
    }
}
