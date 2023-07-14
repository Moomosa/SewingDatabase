using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json;

namespace FrontEnd.Pages.Data.Elastic.Elastic
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
        public SewingModels.Models.Elastic Elastic { get; set; } = default!;
        [BindProperty]
        public List<ElasticTypes> ElasticTypes { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _apiService == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var elastic = await _apiService.GetSingleItem<SewingModels.Models.Elastic>(id.Value, userId);
            if (elastic == null)
                return NotFound();

            Elastic = elastic;

            ElasticTypes = await _apiService.GetRecordsForUser<ElasticTypes>("ElasticTypes", userId);

            HttpContext.Session.SetString("ElasticTypeData", JsonConvert.SerializeObject(ElasticTypes));

            ViewData["ElasticTypeID"] = new SelectList(ElasticTypes, "ID", "Type");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var serializedTypes = HttpContext.Session.GetString("ElasticTypeData");
            ElasticTypes = JsonConvert.DeserializeObject<List<ElasticTypes>>(serializedTypes);

            Elastic.ElasticType = ElasticTypes.FirstOrDefault(et => et.ID == Elastic.ElasticTypeID);

            ModelState.Remove("Elastic.ElasticType");

            if (!ModelState.IsValid)
                return Page();

            try
            {
                bool updated = await _apiService.UpdateItem<SewingModels.Models.Elastic>(Elastic.ID, Elastic);
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
