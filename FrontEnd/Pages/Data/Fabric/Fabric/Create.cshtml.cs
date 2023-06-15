using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BackendDatabase.Data;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Fabric.Fabric
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
        ViewData["FabricBrandID"] = new SelectList(_context.FabricBrand, "ID", "FullName");
        ViewData["FabricTypeID"] = new SelectList(_context.FabricTypes, "ID", "Type");
            return Page();
        }

        [BindProperty]
        public SewingModels.Models.Fabric Fabric { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Fabric == null || Fabric == null)
            {
                return Page();
            }

            _context.Fabric.Add(Fabric);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
