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

namespace FrontEnd.Pages.Data.Thread.Color
{
    [Authorize(Roles = "User,Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApiService _apiService;

        [BindProperty]
        public ThreadColor ThreadColor { get; set; } = default!;
        [BindProperty]
        public List<ThreadColorFamily> ThreadColorFamily {get;set;}

        public CreateModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> OnGet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ThreadColorFamily = await _apiService.GetRecordsForUser<ThreadColorFamily>("ThreadColorFamily", userId);

            HttpContext.Session.SetString("ThreadColorFamilyData", JsonConvert.SerializeObject(ThreadColorFamily));

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var serializedFamily = HttpContext.Session.GetString("ThreadColorFamilyData");
            ThreadColorFamily = JsonConvert.DeserializeObject<List<ThreadColorFamily>>(serializedFamily);

            ThreadColor.ColorFamily = ThreadColorFamily.FirstOrDefault(tcf => tcf.ID == ThreadColor.ColorFamilyID);

            ModelState.Remove("ThreadColor.ColorFamily");

            if (!ModelState.IsValid || _apiService == null)
                return Page();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            HttpResponseMessage response = await _apiService.PostNewItem(ThreadColor, "/api/ThreadColor", userId);

            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.Remove("TColor");
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to create item");
                return Page();
            }            
        }
    }
}
