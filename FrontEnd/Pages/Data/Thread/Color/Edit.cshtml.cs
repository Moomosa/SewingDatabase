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

namespace FrontEnd.Pages.Data.Thread.Color
{
    [Authorize(Roles = "User,Admin")]
    public class EditModel : PageModel
    {
        private readonly ApiService _apiService;

        public EditModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public ThreadColor ThreadColor { get; set; } = default!;
        [BindProperty]
        public List<ThreadColorFamily> ThreadColorFamily { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _apiService == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var threadcolor = await _apiService.GetSingleItem<ThreadColor>(id.Value, userId);
            if (threadcolor == null)
                return NotFound();

            ThreadColor = threadcolor;

            ThreadColorFamily = await _apiService.GetRecordsForUser<ThreadColorFamily>("ThreadColorFamily", userId);

            HttpContext.Session.SetString("ThreadColorFamilyData", JsonConvert.SerializeObject(ThreadColorFamily));

            ViewData["ColorFamilyID"] = new SelectList(ThreadColorFamily, "ID", "ColorFamily");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var serializedColorFamily = HttpContext.Session.GetString("ThreadColorFamilyData");
            ThreadColorFamily = JsonConvert.DeserializeObject<List<ThreadColorFamily>>(serializedColorFamily);

            ThreadColor.ColorFamily = ThreadColorFamily.FirstOrDefault(tcf => tcf.ID == ThreadColor.ColorFamilyID);

            ModelState.Remove("ThreadColor.ColorFamily");

            if (!ModelState.IsValid)
                return Page();
            try
            {
                bool updated = await _apiService.UpdateItem<ThreadColor>(ThreadColor.ID, ThreadColor);
                if (!updated)
                    return NotFound();

                return RedirectToPage("./Index");
            }
            catch
            {
                return StatusCode(500, "An error occured while updating the item.");
            }
        }
    }
}
