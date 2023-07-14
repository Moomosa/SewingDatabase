using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models.Thread;
using Newtonsoft.Json;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Thread.Thread
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
        public SewingModels.Models.Thread Thread { get; set; } = default!;
        [BindProperty]
        public List<ThreadTypes> ThreadTypes { get; set; }
        [BindProperty]
        public List<ThreadColor> ThreadColors { get; set; }
        [BindProperty]
        public List<ThreadColorFamily> ColorFamilies { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _apiService == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var thread = await _apiService.GetSingleItem<SewingModels.Models.Thread>(id.Value, userId);
            if (thread == null)
                return NotFound();

            Thread = thread;

            ThreadTypes = await _apiService.GetRecordsForUser<ThreadTypes>("ThreadTypes", userId);
            ThreadColors = await _apiService.GetRecordsForUser<ThreadColor>("ThreadColor", userId);
            ColorFamilies = await _apiService.GetRecordsForUser<ThreadColorFamily>("ThreadColorFamily", userId);

            HttpContext.Session.SetString("ThreadTypeData", JsonConvert.SerializeObject(ThreadTypes));
            HttpContext.Session.SetString("ThreadColorData", JsonConvert.SerializeObject(ThreadColors));
            HttpContext.Session.SetString("ThreadFamilyData", JsonConvert.SerializeObject(ColorFamilies));

            ViewData["ThreadTypeID"] = new SelectList(ThreadTypes, "ID", "Name");
            ViewData["ColorID"] = new SelectList(ThreadColors, "ID", "Color");
            ViewData["ColorFamilyID"] = new SelectList(ColorFamilies, "ID", "ColorFamily");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var serializedTypes = HttpContext.Session.GetString("ThreadTypeData");
            ThreadTypes = JsonConvert.DeserializeObject<List<ThreadTypes>>(serializedTypes);

            var serializedColors = HttpContext.Session.GetString("ThreadColorData");
            ThreadColors = JsonConvert.DeserializeObject<List<ThreadColor>>(serializedColors);

            var serializedFamilies = HttpContext.Session.GetString("ThreadFamilyData");
            ColorFamilies = JsonConvert.DeserializeObject<List<ThreadColorFamily>>(serializedFamilies);

            Thread.ThreadType = ThreadTypes.FirstOrDefault(tt => tt.ID == Thread.ThreadTypeID);
            Thread.Color = ThreadColors.FirstOrDefault(tc => tc.ID == Thread.ColorID);
            Thread.ColorFamily = ColorFamilies.FirstOrDefault(cf => cf.ID == Thread.Color.ColorFamilyID);
            Thread.ColorFamilyID = Thread.Color.ColorFamilyID;

            ModelState.Remove("Thread.ThreadType");
            ModelState.Remove("Thread.Color");
            ModelState.Remove("Thread.ColorFamily");

            if (!ModelState.IsValid)
                return Page();
            try
            {
                bool updated = await _apiService.UpdateItem<SewingModels.Models.Thread>(Thread.ID, Thread);
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
