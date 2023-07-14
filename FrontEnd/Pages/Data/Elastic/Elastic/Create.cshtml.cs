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

namespace FrontEnd.Pages.Data.Elastic.Elastic
{
    [Authorize(Roles = "User,Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApiService _apiService;

        [BindProperty]
        public SewingModels.Models.Elastic Elastic { get; set; } = default!;
        [BindProperty]
        public List<ElasticTypes> ElasticTypes { get; set; }

        public CreateModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> OnGet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ElasticTypes = await _apiService.GetRecordsForUser<ElasticTypes>("ElasticTypes", userId);

            HttpContext.Session.SetString("ElasticTypeData", JsonConvert.SerializeObject(ElasticTypes));

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var serializedTypes = HttpContext.Session.GetString("ElasticTypeData");
            ElasticTypes = JsonConvert.DeserializeObject<List<ElasticTypes>>(serializedTypes);

            Elastic.ElasticType = ElasticTypes.FirstOrDefault(et => et.ID == Elastic.ElasticTypeID);

            ModelState.Remove("Elastic.ElasticType");

            if (!ModelState.IsValid || _apiService == null)
                return Page();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            HttpResponseMessage response = await _apiService.PostNewItem(Elastic, "/api/Elastic", userId);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("./Index");
            else
            {
                ModelState.AddModelError("", "Failed to create item");
                return Page();
            }
        }
    }
}
