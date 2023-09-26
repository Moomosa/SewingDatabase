using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SewingModels.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Elastic.Elastic
{
    [Authorize(Roles = "User,Admin")]
    public class CreateModel : BaseCreateModel<SewingModels.Models.Elastic>
    {
        [BindProperty]
        public List<ElasticTypes> ElasticTypes { get; set; }

		public CreateModel(IHttpContextAccessor httpContextAccessor)
			: base(httpContextAccessor)
		{
		}

		public override async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ElasticTypes = await ApiService.GatherAllRecords<ElasticTypes>("ElasticTypes", userId, 20);

            await base.OnGetAsync();
            return Page();
        }


        public override async Task<IActionResult> OnPostAsync()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Item.ElasticType = await ApiService.GetSingleItem<ElasticTypes>(Item.ElasticTypeID, userId);

            ModelState.Remove("Item.ElasticType");

			return await base.OnPostAsync();
		}
	}
}
