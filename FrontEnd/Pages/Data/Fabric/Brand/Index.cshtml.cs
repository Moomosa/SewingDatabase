﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Text;

namespace FrontEnd.Pages.Data.Fabric.Brand
{
	[Authorize(Roles = "User,Admin")]
	public class IndexModel : PageModel
	{
		private readonly ApiService _apiService;

		public IndexModel(ApiService apiService)
		{
			_apiService = apiService;
		}

		public IList<FabricBrand> FabricBrand { get; set; } = default!;
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

			if (!HttpContext.Session.TryGetValue("FBrandTotalRecords", out byte[] totalRecordsBytes))
			{
				TotalRecords = await _apiService.GetTotalRecords<FabricBrand>("FabricBrand", userId);
				HttpContext.Session.SetObjectAsJson("FBrandTotalRecords", TotalRecords);
			}
			else
				TotalRecords = JsonConvert.DeserializeObject<int>(Encoding.UTF8.GetString(totalRecordsBytes));


			int lastPageVisited = HttpContext.Session.GetObjectFromJson<int>("FBrandLastPage");

			string referrer = HttpContext.Request.Headers["Referer"];
			if (!string.IsNullOrEmpty(referrer) && referrer.Contains("Fabric/Brand"))
			{
				if (CurrentPage != lastPageVisited)
				{
					FabricBrand = await _apiService.GetPagedRecords<FabricBrand>("FabricBrand", userId, CurrentPage, PageSize);
					HttpContext.Session.SetObjectAsJson("FBrands", FabricBrand);
					HttpContext.Session.SetObjectAsJson("FBrandLastPage", CurrentPage);
				}
				else
					FabricBrand = HttpContext.Session.GetObjectFromJson<IList<FabricBrand>>("FBrands");
			}
			else
			{
				if (lastPageVisited != 0)
				{
					CurrentPage = lastPageVisited;
					FabricBrand = HttpContext.Session.GetObjectFromJson<IList<FabricBrand>>("FBrands");
				}
				else
				{
					FabricBrand = await _apiService.GetPagedRecords<FabricBrand>("FabricBrand", userId, CurrentPage, PageSize);
					HttpContext.Session.SetObjectAsJson("FBrands", FabricBrand);
				}
			}

			return Page();
		}
	}
}
