using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models.Thread;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Thread.Color
{
    [Authorize(Roles = "User,Admin")]
    public class EditModel : BaseEditModel<ThreadColor>
    {
        public EditModel(IHttpContextAccessor httpContextAccessor):base(httpContextAccessor)
        {
        }

        [BindProperty]
        public List<ThreadColorFamily> ThreadColorFamily { get; set; } = default!;

        public override async Task<IActionResult> OnGetAsync(int? id)
        {
            await base.OnGetAsync(id);

			string userId = FrontHelpers.GetUserId(User);

			ThreadColorFamily = await ApiService.GatherAllRecords<ThreadColorFamily>("ThreadColorFamily", userId, 20);

            ViewData["ColorFamilyID"] = new SelectList(ThreadColorFamily, "ID", "ColorFamily");
            return Page();
        }

        public override async Task<IActionResult> OnPostAsync(int? id)
        {
			string userId = FrontHelpers.GetUserId(User);

            Item.ColorFamily = await ApiService.GetSingleItem<ThreadColorFamily>(Item.ColorFamilyID, userId);

            ModelState.Remove("Item.ColorFamily");

            return await base.OnPostAsync(id);        }
    }
}
