using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Models.Thread;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Thread.Color
{
    [Authorize(Roles = "User,Admin")]
    public class CreateModel : BaseCreateModel<ThreadColor>
    {
        [BindProperty]
        public List<ThreadColorFamily> ThreadColorFamily {get;set;}

		public CreateModel(IHttpContextAccessor httpContextAccessor)
			: base(httpContextAccessor)
		{
		}

		public override async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ThreadColorFamily = await ApiService.GatherAllRecords<ThreadColorFamily>("ThreadColorFamily", userId, 20);

            return await base.OnGetAsync();            
        }

        public override async Task<IActionResult> OnPostAsync()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Item.ColorFamily = await ApiService.GetSingleItem<ThreadColorFamily>(Item.ColorFamilyID, userId);

            ModelState.Remove("Item.ColorFamily");

            return await base.OnPostAsync();
        }
    }
}
