using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Fabric.Brand
{
    public class EditModel : PageModel
    {
        private readonly BackendDatabase.Data.BackendDatabaseContext _context;

        public EditModel(BackendDatabase.Data.BackendDatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FabricBrand FabricBrand { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.FabricBrand == null)
            {
                return NotFound();
            }

            var fabricbrand =  await _context.FabricBrand.FirstOrDefaultAsync(m => m.ID == id);
            if (fabricbrand == null)
            {
                return NotFound();
            }
            FabricBrand = fabricbrand;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(FabricBrand).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FabricBrandExists(FabricBrand.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FabricBrandExists(int id)
        {
          return (_context.FabricBrand?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
