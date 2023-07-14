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
using Newtonsoft.Json;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Misc.MiscObjects
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
        public SewingModels.Models.MiscObjects MiscObjects { get; set; } = default!;
        [BindProperty]
        public List<MiscItemType> MiscItemTypes { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _apiService == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var miscobjects = await _apiService.GetSingleItem<SewingModels.Models.MiscObjects>(id.Value, userId);
            if (miscobjects == null)
                return NotFound();

            MiscObjects = miscobjects;

            MiscItemTypes = await _apiService.GetRecordsForUser<MiscItemType>("MiscItemType", userId);

            HttpContext.Session.SetString("MiscItemData", JsonConvert.SerializeObject(MiscItemTypes));

            ViewData["ItemTypeID"] = new SelectList(MiscItemTypes, "ID", "Item");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var serializedItems = HttpContext.Session.GetString("MiscItemData");
            MiscItemTypes = JsonConvert.DeserializeObject<List<MiscItemType>>(serializedItems);
            MiscObjects.ItemType = MiscItemTypes.FirstOrDefault(mit => mit.ID == MiscObjects.ItemTypeID);

            ModelState.Remove("MiscObjects.ItemType");

            if (!ModelState.IsValid)
                return Page();

            try
            {
                bool updated = await _apiService.UpdateItem<SewingModels.Models.MiscObjects>(MiscObjects.ID, MiscObjects);
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
