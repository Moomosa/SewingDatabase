using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BackendDatabase.Data;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Fabric.Brand
{
    public class CreateModel : PageModel
    {
        private readonly BackendDatabase.Data.BackendDatabaseContext _context;

        public CreateModel(BackendDatabase.Data.BackendDatabaseContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public FabricBrand FabricBrand { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.FabricBrand == null || FabricBrand == null)
            {
                return Page();
            }

            _context.FabricBrand.Add(FabricBrand);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
