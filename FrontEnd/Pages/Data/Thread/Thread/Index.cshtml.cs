﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Thread.Thread
{
	[Authorize(Roles = "User,Admin")]
	public class IndexModel : PageModel
	{
		private readonly ApiService _apiService;

		public IndexModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public IList<SewingModels.Models.Thread> Thread { get; set; } = default!;
		[BindProperty(SupportsGet = true)]
		public int CurrentPage { get; set; } = 1;
		public int PageSize { get; set; } = 1;    //Set this to number of rows seen on page
		public int TotalRecords { get; set; } = -1;
		public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalRecords, PageSize));
		public bool ShowPrevious => CurrentPage > 1;
		public bool ShowNext => CurrentPage < TotalPages;
		public bool ShowFirst => CurrentPage != 1;
		public bool ShowLast => CurrentPage != TotalPages;

		public async Task<IActionResult> OnGetAsync()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null) return RedirectToPage("/Account/Login");

			if (!HttpContext.Session.TryGetValue("ThreadTotalRecords", out byte[] totalRecordsBytes))
			{
				TotalRecords = await _apiService.GetTotalRecords<SewingModels.Models.Thread>("Thread", userId);
				HttpContext.Session.SetObjectAsJson("ThreadTotalRecords", TotalRecords);
			}
			else
				TotalRecords = JsonConvert.DeserializeObject<int>(Encoding.UTF8.GetString(totalRecordsBytes));

			int lastPageVisited = HttpContext.Session.GetObjectFromJson<int>("ThreadLastPage");

			string referrer = HttpContext.Request.Headers["Referer"];
			if (!string.IsNullOrEmpty(referrer) && referrer.Contains("Thread/Thread"))
			{
				if (CurrentPage != lastPageVisited) //This hits when changing pagination page
				{
					Thread = await _apiService.GetPagedRecords<SewingModels.Models.Thread>("Thread", userId, CurrentPage, PageSize);
					HttpContext.Session.SetObjectAsJson("Threads", Thread);
					HttpContext.Session.SetObjectAsJson("ThreadLastPage", CurrentPage);
				}
				else //A refresh possibility
					Thread = HttpContext.Session.GetObjectFromJson<IList<SewingModels.Models.Thread>>("Threads");
			}
			else
			{
				if (lastPageVisited != 0) //This hits when coming from not this page
				{
					CurrentPage = lastPageVisited;
					Thread = HttpContext.Session.GetObjectFromJson<IList<SewingModels.Models.Thread>>("Threads");
				}
				else //This hits the first time coming to the page
				{
					Thread = await _apiService.GetPagedRecords<SewingModels.Models.Thread>("Thread", userId, CurrentPage, PageSize);
					HttpContext.Session.SetObjectAsJson("Threads", Thread);
				}
			}

			return Page();
		}
	}
}
